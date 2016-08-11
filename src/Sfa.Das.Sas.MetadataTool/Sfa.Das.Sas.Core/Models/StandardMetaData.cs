using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Models
{
    public class StandardMetaData
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        public int NotionalEndLevel { get; set; }

        public string StandardPdfUrl { get; set; }

        public string AssessmentPlanPdfUrl { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }

        public double SectorSubjectAreaTier1 { get; set; }

        public double SectorSubjectAreaTier2 { get; set; }
    }
}
