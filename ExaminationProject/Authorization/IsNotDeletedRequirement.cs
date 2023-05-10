using ExaminationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExaminationProject.Authorization
{
    public class IsNotDeletedRequirement : IAuthorizationRequirement
    {
        public IsNotDeletedRequirement() {}
        public class IsNotDeletedRequirementHandler : AuthorizationHandler<IsNotDeletedRequirement>
        {
            private readonly UserManager<User> _userManager;

            public IsNotDeletedRequirementHandler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsNotDeletedRequirement requirement)
            {
                var user = await _userManager.GetUserAsync(context.User);

                if (user != null && user.IsDeleted)
                {
                    context.Fail();
                    return;
                }

                context.Succeed(requirement);
            }
        }
    }
}
