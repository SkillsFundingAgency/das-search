using NUnit.Framework;
using Sfa.Das.Sas.Web.Helpers;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Helpers
{
    [TestFixture]
    public class UrlEncoderTests
    {
        [TestCase("WOODSPEEN TRAINING LIMITED", "woodspeen-training-limited")]
        [TestCase("5 E LTD.", "5e-ltd")]
        [TestCase("TRN(TRAIN) LTD.", "trn-train-ltd")]
        [TestCase("ACTIVE LEARNING & DEVELOPMENT LIMITED", "active-learning-and-development-limited")]
        [TestCase("BARKING & DAGENHAM LONDON BOROUGH COUNCIL", "barking-and-dagenham-london-borough-council")]
        [TestCase("BARNARDO'S", "barnardos")]
        [TestCase("BASSETLAW TRAINING AGENCY(PROPERTIES) LIMITED", "bassetlaw-training-agency-properties-limited")]
        [TestCase("BERKSHIRE COLLEGE OF AGRICULTURE, THE(BCA)", "berkshire-college-of-agriculture-the-bca")]
        [TestCase("BOURNEMOUTH AND POOLE COLLEGE, THE", "bournemouth-and-poole-college-the")]
        [TestCase("C.M.S.VOCATIONAL TRAINING LIMITED", "cms-vocational-training-limited")]
        [TestCase("EALING, HAMMERSMITH & WEST LONDON COLLEGE", "ealing-hammersmith-and-west-london-college")]
        [TestCase("EAST MIDLANDS CHAMBER(DERBYSHIRE, NOTTINGHAMSHIRE, LEICESTERSHIRE)", "east-midlands-chamber-derbyshire-nottinghamshire-leicestershire")]
        [TestCase("E.QUALITY TRAINING LIMITED", "e-quality-training-limited")]
        [TestCase("E.Q.V. (UK) LIMITED", "eqv-uk-limited")]
        [TestCase("FASHION - ENTER LTD", "fashion-enter-ltd")]
        [TestCase("MARITIME + ENGINEERING COLLEGE NORTH WEST", "maritime-and-engineering-college-north-west")]
        [TestCase(" N & B TRAINING LTD", "n-and-b-training-ltd")]
        [TestCase("GREENBANK PROJECT(THE)", "greenbank-project-the")]
        [TestCase("L.I.T.S.LIMITED", "lits-limited")]
        [TestCase("NORTH LANCS.TRAINING GROUP LIMITED(THE)", "north-lancs-training-group-limited-the")]
        [TestCase("SOUTHAMPTON ENGINEERING TRAINING ASSOCIATION LIMITED(THE)", "southampton-engineering-training-association-limited-the")]
        [TestCase("SHEFFIELD COLLEGE, THE", "sheffield-college-the")]
        [TestCase("ST.PATRICK'S INTERNATIONAL COLLEGE LIMITED", "st-patricks-international-college-limited")]
        [TestCase("GUY'S AND ST THOMAS' NHS FOUNDATION TRUST", "guys-and-st-thomas-nhs-foundation-trust")]
        [TestCase("Name with [ weird $ characters", "name-with-\\[-weird-\\$-characters")]
        public void ShouldReturnStringModifiedForUrlUsage(string words, string modifiedwords)
        {
            var actual = new UrlEncoder().EncodeTextForUri(words);
            Assert.AreEqual(actual, modifiedwords);
        }
    }
}
