using ApplicantTracking.Domain.AuditTrail;

using Moq;

namespace CommonTestUtilities.AuditTrail;

public class AuditTrailRecorderBuilder
{
    public static IAuditTrailRecorder Build()
    {
        var auditTrailRecorder = new Mock<IAuditTrailRecorder>();

        return auditTrailRecorder.Object;
    }
}
