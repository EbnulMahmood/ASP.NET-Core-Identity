using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public int ProbationMonths { get; private set; }

        public HRManagerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }
    }

    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            const string employmentDate = "EmploymentDate";
            if (!context.User.HasClaim(x => x.Type == employmentDate)) return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == employmentDate).Value);
            var period = DateTime.Now - empDate;

            if (period.Days > 30 * requirement.ProbationMonths) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
