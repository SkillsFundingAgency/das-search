namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class StubCourseDirectoryClient : IGetApprenticeshipProviders
    {
        private const string Json = @"[
  {
    ""ukprn"": -98154690,
    ""phone"": ""a"",
    ""employerSatisfaction"": 2955928.029325586,
    ""frameworks"": [
      {
        ""frameworkCode"": 84177974,
        ""pathwayCode"": -16966867,
        ""level"": -98942116
      }
    ],
    ""name"": ""delectus corrupti voluptatem""
  },
  {
    ""ukprn"": -2344398,
    ""name"": ""error"",
    ""marketingInfo"": ""voluptatibus quaerat facilis"",
    ""learnerSatisfaction"": -70627046.81376992,
    ""standards"": [
      {
        ""standardCode"": 12371523,
        ""contact"": {
          ""contactUsUrl"": ""dolorem autem reiciendis nesciunt qui"",
          ""phone"": ""perferendis unde error sit saepe""
        },
        ""standardInfoUrl"": ""distinctio"",
        ""locations"": [
          {
            ""id"": -7616492,
            ""standardInfoUrl"": ""veritatis distinctio et porro nam"",
            ""deliveryModes"": [
              ""dolorem est unde""
            ],
            ""marketingInfo"": ""eos ut voluptatum harum quae"",
            ""radius"": -17951621
          },
          {
            ""id"": -15726413,
            ""radius"": -45434138
          },
          {
            ""id"": -46366347,
            ""marketingInfo"": ""tempore voluptate qui et"",
            ""standardInfoUrl"": ""aliquam totam""
          },
          {
            ""id"": -39758100,
            ""deliveryModes"": [
              ""nemo id non quae""
            ]
          }
        ]
      },
      {
        ""standardCode"": -33863758,
        ""contact"": {
          ""phone"": ""minus illum vel"",
          ""email"": ""sint quam"",
          ""contactUsUrl"": ""voluptate doloremque hic""
        },
        ""marketingInfo"": ""dolorem""
      },
      {
        ""standardCode"": -26829376,
        ""locations"": [
          {
            ""id"": 15822536,
            ""standardInfoUrl"": ""eveniet vel laborum est""
          },
          {
            ""id"": 8878181,
            ""deliveryModes"": [
              ""et quam in dolore illo"",
              ""et quam earum cumque fugit"",
              ""soluta et rerum sed"",
              ""rerum et""
            ],
            ""marketingInfo"": ""dignissimos"",
            ""standardInfoUrl"": ""sint ab""
          }
        ],
        ""marketingInfo"": ""distinctio voluptatem suscipit omnis"",
        ""standardInfoUrl"": ""maiores nobis"",
        ""contact"": {
          ""email"": ""atque sit"",
          ""phone"": ""et consequuntur"",
          ""contactUsUrl"": ""officiis beatae""
        }
      }
    ],
    ""phone"": ""quo enim""
  },
  {
    ""ukprn"": -53813791,
    ""website"": ""beatae est"",
    ""employerSatisfaction"": 13901514.538655698,
    ""frameworks"": [
      {
        ""frameworkCode"": 12436324,
        ""pathwayCode"": -16497010,
        ""level"": -51406307
      }
    ]
  },
  {
    ""ukprn"": 29968092,
    ""website"": ""minima"",
    ""marketingInfo"": ""et voluptas qui quia""
  }
]";

        public Task<IEnumerable<Provider>> GetApprenticeshipProvidersAsync()
        {
            return Task.FromResult(Retrieve());
        }

        private IEnumerable<Provider> Retrieve()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Provider>>(Json);
        }
    }
}