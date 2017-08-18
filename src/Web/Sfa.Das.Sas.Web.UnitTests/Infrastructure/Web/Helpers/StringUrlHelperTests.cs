using NUnit.Framework;
using Sfa.Das.Sas.Web.Helpers;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Helpers
{
    [TestFixture]
    public class StringUrlHelperTests
    {

      /*  5 E LTD.
    TRN(TRAIN) LTD.
    ACTIVE LEARNING & DEVELOPMENT LIMITED
    BARKING & DAGENHAM LONDON BOROUGH COUNCIL
    AMERSHAM & WYCOMBE COLLEGE
    ASSET TRAINING & CONSULTANCY LIMITED
    ASTON RECRUITMENT & TRAINING LIMITED
    BARNARDO'S
    BARNET & SOUTHGATE COLLEGE
    BASSETLAW TRAINING AGENCY(PROPERTIES) LIMITED
    BERKSHIRE COLLEGE OF AGRICULTURE, THE(BCA)
    BOURNEMOUTH AND POOLE COLLEGE, THE
    "BMC (BROOKSBY MELTON COLLEGE)"
    "C.M.S. VOCATIONAL TRAINING LIMITED"
    DC TRAINING & DEVELOPMENT SERVICES LIMITED
    EALING, HAMMERSMITH & WEST LONDON COLLEGE
    "EAST MIDLANDS CHAMBER (DERBYSHIRE, NOTTINGHAMSHIRE, LEICESTERSHIRE)"
    stringToProcess	"E.QUALITY TRAINING LIMITED"	string
    "E.Q.V. (UK) LIMITED"
    FASHION - ENTER LTD
    MARITIME + ENGINEERING COLLEGE NORTH WEST
    N & B TRAINING LTD
    SOUTH & CITY COLLEGE BIRMINGHAM*/

        [TestCase("5 E LTD.", "5-e-ltd")]
        [TestCase("TRN(TRAIN) LTD.", "trn-train-ltd")]

        public void ShouldReturnStringModifiedForUrlUsage(string words, string modifiedwords)
        {
            var actual = new StringUrlHelper().ModifyStringForUrlUsage(words);
            Assert.AreEqual(actual, modifiedwords);

        }
    }
}
