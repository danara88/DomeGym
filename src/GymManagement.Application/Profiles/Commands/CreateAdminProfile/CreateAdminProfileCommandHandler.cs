using ErrorOr;

using MediatR;

using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;

namespace GymManagement.Application.Profiles.Commands.CreateAdminProfile;

/// <summary>
/// Create admin profile command handler
/// </summary>
public class CreateAdminProfileCommandHandler : IRequestHandler<CreateAdminProfileCommand, ErrorOr<Guid>>
{
    private readonly IAdminsRepository _adminsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserProvider _currentUser;

    public CreateAdminProfileCommandHandler(IAdminsRepository adminRepository, IUsersRepository usersRepository, IUnitOfWork unitOfWork, ICurrentUserProvider currentUser)
    {
        _adminsRepository = adminRepository;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateAdminProfileCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _currentUser.GetCurrentUser();

        if (currentUser.Id != command.UserId)
        {
            // return Error.Unauthorized(description: "User is forbidden from taking this action.");
            return Error.Failure(description: "User is forbidden from taking this action.");
        }

        var user = await _usersRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        var createAdminProfileResult = user.CreateAdminProfile();
        var admin = new Admin(userId: user.Id, id: createAdminProfileResult.Value);

        await _usersRepository.UpdateAsync(user);
        await _adminsRepository.AddAdminAsync(admin);
        await _unitOfWork.CommitChangesAsync();

        return createAdminProfileResult;
    }
}
