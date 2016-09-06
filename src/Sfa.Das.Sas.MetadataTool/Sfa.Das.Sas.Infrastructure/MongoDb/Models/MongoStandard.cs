namespace Sfa.Das.Sas.Infrastructure.MongoDb.Models
{
    using System;
    using System.Collections.Generic;

    using Sfa.Das.Sas.ApplicationServices.Models;

    internal class Standard : IDataBaseType
    {
        public Guid Id { get; set; }

        public string DocumentVersion { get; set; }

        public int ApprenticeshipId { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        public int NotionalEndLevel { get; set; }

        public string StandardPdfUrl { get; set; }

        public string AssessmentPlanPdfUrl { get; set; }

        public MongoTypicalLength TypicalLength { get; set; }

        public string EntryRequirements { get; set; }

        public string WhatApprenticesWillLearn { get; set; }

        public string Qualifications { get; set; }

        public string ProfessionalRegistration { get; set; }

        public string OverviewOfRole { get; set; }

        public double SectorSubjectAreaTier1 { get; set; }

        public double SectorSubjectAreaTier2 { get; set; }
    }
}