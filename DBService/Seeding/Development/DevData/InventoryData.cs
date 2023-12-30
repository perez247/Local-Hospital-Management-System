using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development.DevData
{
    internal static class InventoryData
    {
        public static string JsonData = @"

        [
  {
    ""id"": ""0a85d024-15db-41a8-9077-df01cfffd5c2"",
    ""quantity"": 807,
    ""notifyWhenLow"": false,
    ""howLow"": 19,
    ""name"": ""Borax""
  },
  {
    ""id"": ""8e4b0464-427a-4e34-b0b7-ac18b991092b"",
    ""quantity"": 880,
    ""notifyWhenLow"": true,
    ""howLow"": 7,
    ""name"": ""Bortezomib""
  },
  {
    ""id"": ""560c033f-c6f2-4d68-9994-679f130bce7a"",
    ""quantity"": 808,
    ""notifyWhenLow"": false,
    ""howLow"": 17,
    ""name"": ""LAMOTRIGINE""
  },
  {
    ""id"": ""e1cc7979-aec9-42d5-8ae9-1d72693b2e63"",
    ""quantity"": 488,
    ""notifyWhenLow"": false,
    ""howLow"": 10,
    ""name"": ""Oxaliplatin""
  },
  {
    ""id"": ""670c92e1-254e-4d44-b65c-f9e8907facdb"",
    ""quantity"": 412,
    ""notifyWhenLow"": true,
    ""howLow"": 8,
    ""name"": ""Tramadol Hydrochloride and Acetaminophen""
  },
  {
    ""id"": ""132dc42c-5df5-4455-a19d-9920e340d109"",
    ""quantity"": 36,
    ""notifyWhenLow"": false,
    ""howLow"": 9,
    ""name"": ""Laxative""
  },
  {
    ""id"": ""6c8531c1-f9d4-4039-98fa-fb66ee8ef77f"",
    ""quantity"": 70,
    ""notifyWhenLow"": true,
    ""howLow"": 4,
    ""name"": ""Anticavity Fluoride Rinse""
  },
  {
    ""id"": ""7d8e1003-3e55-45c1-90e0-6151d002c4d0"",
    ""quantity"": 857,
    ""notifyWhenLow"": false,
    ""howLow"": 9,
    ""name"": ""METOPROLOL SUCCINATE""
  },
  {
    ""id"": ""85abbc05-4936-4ecb-a504-92e9ac3bf2f4"",
    ""quantity"": 350,
    ""notifyWhenLow"": true,
    ""howLow"": 11,
    ""name"": ""Flumazenil""
  },
  {
    ""id"": ""3b7d4392-a144-498e-8733-cf29a58baacb"",
    ""quantity"": 41,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""Actaea Spicata""
  },
  {
    ""id"": ""4ab1e2bf-0940-48d7-b8e1-76de56a6700d"",
    ""quantity"": 66,
    ""notifyWhenLow"": false,
    ""howLow"": 5,
    ""name"": ""Metoprolol Succinate""
  },
  {
    ""id"": ""4e877306-bf4f-4497-a744-6047068b7a47"",
    ""quantity"": 905,
    ""notifyWhenLow"": true,
    ""howLow"": 2,
    ""name"": ""Enalapril Maleate""
  },
  {
    ""id"": ""85e34495-93f5-4e77-94e7-27d9f1fb5ff2"",
    ""quantity"": 647,
    ""notifyWhenLow"": false,
    ""howLow"": 15,
    ""name"": ""Alcohol Prep Pads""
  },
  {
    ""id"": ""3eecc9fc-1ab2-41b1-b09e-217d835a19c9"",
    ""quantity"": 406,
    ""notifyWhenLow"": true,
    ""howLow"": 8,
    ""name"": ""Cisatracurium Besylate""
  },
  {
    ""id"": ""5d999ae2-6252-4b96-b38a-264c15984df6"",
    ""quantity"": 28,
    ""notifyWhenLow"": true,
    ""howLow"": 6,
    ""name"": ""Itch Relief""
  },
  {
    ""id"": ""0a5e10bc-77e5-4b3d-a313-91f7e13b5c75"",
    ""quantity"": 281,
    ""notifyWhenLow"": false,
    ""howLow"": 18,
    ""name"": ""Sumatriptan Succinate""
  },
  {
    ""id"": ""ccc12819-85c4-40ac-8f09-578a6713159d"",
    ""quantity"": 534,
    ""notifyWhenLow"": true,
    ""howLow"": 3,
    ""name"": ""Argentum nitricum""
  },
  {
    ""id"": ""21a41779-4fe9-436a-bfa2-dc4ccfb54f5d"",
    ""quantity"": 420,
    ""notifyWhenLow"": true,
    ""howLow"": 20,
    ""name"": ""Sodium Fluoride F 18""
  },
  {
    ""id"": ""818c2807-b962-46e4-878d-63a10413fa0a"",
    ""quantity"": 123,
    ""notifyWhenLow"": true,
    ""howLow"": 7,
    ""name"": ""Quetiapine Fumarate""
  },
  {
    ""id"": ""af553142-f240-4b39-b3ac-942faf629523"",
    ""quantity"": 54,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""Cefuroxime axetil""
  },
  {
    ""id"": ""8ecac4f8-efb0-410e-b917-90c03ec84758"",
    ""quantity"": 575,
    ""notifyWhenLow"": true,
    ""howLow"": 20,
    ""name"": ""RISPERIDONE""
  },
  {
    ""id"": ""517f7ac0-90fc-408d-be87-fab9dae4a748"",
    ""quantity"": 993,
    ""notifyWhenLow"": true,
    ""howLow"": 17,
    ""name"": ""Phenytoin Sodium""
  },
  {
    ""id"": ""5682d079-f6a4-48ed-8879-6cf4e4d01d3b"",
    ""quantity"": 98,
    ""notifyWhenLow"": false,
    ""howLow"": 16,
    ""name"": ""Esomeprazole Magnesium""
  },
  {
    ""id"": ""f0bd748b-332a-4ae7-ab2a-841d7f459fc3"",
    ""quantity"": 392,
    ""notifyWhenLow"": false,
    ""howLow"": 7,
    ""name"": ""Pregabalin""
  },
  {
    ""id"": ""0af040ed-02f6-4d04-9413-e3e69037ff07"",
    ""quantity"": 451,
    ""notifyWhenLow"": true,
    ""howLow"": 13,
    ""name"": ""Docosanol""
  },
  {
    ""id"": ""8eaa9693-8b74-4951-bc55-4372cd7791f8"",
    ""quantity"": 928,
    ""notifyWhenLow"": true,
    ""howLow"": 8,
    ""name"": ""Benzalkonium chloride""
  },
  {
    ""id"": ""a82b5c73-a5df-4a5e-b42b-f4834bfb26e1"",
    ""quantity"": 126,
    ""notifyWhenLow"": true,
    ""howLow"": 13,
    ""name"": ""Alprazolam""
  },
  {
    ""id"": ""f8c30bc4-0f52-41bb-a882-424a452ffd07"",
    ""quantity"": 63,
    ""notifyWhenLow"": false,
    ""howLow"": 16,
    ""name"": ""Clobetasol Propionate""
  },
  {
    ""id"": ""e38510c0-1a7d-4462-a7ce-082045833876"",
    ""quantity"": 543,
    ""notifyWhenLow"": false,
    ""howLow"": 4,
    ""name"": ""BIO SPECTRA ATTITUDE""
  },
  {
    ""id"": ""7892ee45-51f0-44ab-ba43-89c7fbb3001b"",
    ""quantity"": 262,
    ""notifyWhenLow"": false,
    ""howLow"": 2,
    ""name"": ""Senna""
  },
  {
    ""id"": ""dab9da7a-ab65-4030-846b-9d515604d397"",
    ""quantity"": 548,
    ""notifyWhenLow"": true,
    ""howLow"": 2,
    ""name"": ""Clopidogrel""
  },
  {
    ""id"": ""ed8790b8-ee2d-413a-83f5-0dca809aff75"",
    ""quantity"": 690,
    ""notifyWhenLow"": false,
    ""howLow"": 18,
    ""name"": ""Misoprostol""
  },
  {
    ""id"": ""c7943b9a-5f6c-4f00-87d4-b7df03e44f7c"",
    ""quantity"": 754,
    ""notifyWhenLow"": true,
    ""howLow"": 20,
    ""name"": ""Antibacterial""
  },
  {
    ""id"": ""c951efd1-fcfd-4171-a14a-a80cde87f139"",
    ""quantity"": 45,
    ""notifyWhenLow"": false,
    ""howLow"": 3,
    ""name"": ""Nitroglycerin""
  },
  {
    ""id"": ""c4150c09-35e1-41aa-ab43-04d6dc1e6915"",
    ""quantity"": 7,
    ""notifyWhenLow"": false,
    ""howLow"": 7,
    ""name"": ""Fludrocortisone Acetate""
  },
  {
    ""id"": ""f06df4c4-ef75-4b8f-bf3f-d41f40f2ca52"",
    ""quantity"": 846,
    ""notifyWhenLow"": false,
    ""howLow"": 20,
    ""name"": ""montelukast sodium""
  },
  {
    ""id"": ""df416f1f-7ffe-467a-acf5-27cf2c4e2f4f"",
    ""quantity"": 212,
    ""notifyWhenLow"": false,
    ""howLow"": 18,
    ""name"": ""Midazolam Hydrochloride""
  },
  {
    ""id"": ""02f17a7b-6698-47bf-86d4-3cbc45f75394"",
    ""quantity"": 719,
    ""notifyWhenLow"": false,
    ""howLow"": 9,
    ""name"": ""Magnesium Sulfate""
  },
  {
    ""id"": ""de3fe7ce-ebfd-4b44-82f6-e36a7b1f1597"",
    ""quantity"": 710,
    ""notifyWhenLow"": false,
    ""howLow"": 15,
    ""name"": ""Isoniazid""
  },
  {
    ""id"": ""2113734a-47d6-4305-9707-eadc5fc75e8e"",
    ""quantity"": 464,
    ""notifyWhenLow"": false,
    ""howLow"": 8,
    ""name"": ""Glycerin""
  },
  {
    ""id"": ""f7cf3725-4436-4cc8-8cd6-911ac347789a"",
    ""quantity"": 957,
    ""notifyWhenLow"": false,
    ""howLow"": 13,
    ""name"": ""Esomeprazole Magnesium""
  },
  {
    ""id"": ""6372ea75-3e1b-44c5-b2a6-df13edc6fc10"",
    ""quantity"": 92,
    ""notifyWhenLow"": true,
    ""howLow"": 1,
    ""name"": ""Advanced Hand Sanitizer""
  },
  {
    ""id"": ""f50d0f8b-be2b-4664-8354-2a816f48e574"",
    ""quantity"": 208,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""Alcohol Prep""
  },
  {
    ""id"": ""59bf88a5-8d71-47ff-9615-053b9d175bf5"",
    ""quantity"": 288,
    ""notifyWhenLow"": true,
    ""howLow"": 19,
    ""name"": ""Advanced Hand Sanitizer""
  },
  {
    ""id"": ""4d46e00d-a56a-405e-9e42-9e10679f9439"",
    ""quantity"": 821,
    ""notifyWhenLow"": false,
    ""howLow"": 13,
    ""name"": ""Pemetrexed""
  },
  {
    ""id"": ""3398170c-6d81-417f-b10a-d58948dff3aa"",
    ""quantity"": 133,
    ""notifyWhenLow"": true,
    ""howLow"": 2,
    ""name"": ""DONEPEZIL HYDROCHLORIDE""
  },
  {
    ""id"": ""7e3fa50d-1770-4fb2-a9e9-7cc948fa8ad9"",
    ""quantity"": 983,
    ""notifyWhenLow"": false,
    ""howLow"": 20,
    ""name"": ""Allergy""
  },
  {
    ""id"": ""4ac6dd7e-abe8-48b2-88fe-dc1de938bfa0"",
    ""quantity"": 28,
    ""notifyWhenLow"": false,
    ""howLow"": 8,
    ""name"": ""Capecitabine""
  },
  {
    ""id"": ""d8d02275-606e-467c-80b4-25d11827d3e3"",
    ""quantity"": 959,
    ""notifyWhenLow"": false,
    ""howLow"": 20,
    ""name"": ""Quetiapine Fumarate""
  },
  {
    ""id"": ""99f37998-1bff-4720-badc-27f85c223540"",
    ""quantity"": 870,
    ""notifyWhenLow"": false,
    ""howLow"": 11,
    ""name"": ""aripiprazole""
  },
  {
    ""id"": ""eae2262e-c0d6-40ad-8d5c-db589fb09614"",
    ""quantity"": 404,
    ""notifyWhenLow"": false,
    ""howLow"": 17,
    ""name"": ""SERTRALINE HYDROCHLORIDE""
  },
  {
    ""id"": ""28410147-afa4-42be-80db-57a27d845cbf"",
    ""quantity"": 110,
    ""notifyWhenLow"": true,
    ""howLow"": 15,
    ""name"": ""Desonide""
  },
  {
    ""id"": ""b04dd509-c488-4c06-8897-3ea5618bfd11"",
    ""quantity"": 126,
    ""notifyWhenLow"": true,
    ""howLow"": 20,
    ""name"": ""Sotalol Hydrochloride""
  },
  {
    ""id"": ""346979bd-b6d6-4ee4-a681-9a15c993d736"",
    ""quantity"": 426,
    ""notifyWhenLow"": false,
    ""howLow"": 10,
    ""name"": ""Amantadine Hydrochloride""
  },
  {
    ""id"": ""171d3983-6c93-4f21-8838-0cc0a0239c14"",
    ""quantity"": 793,
    ""notifyWhenLow"": true,
    ""howLow"": 17,
    ""name"": ""Terbinafine Hydrochloride""
  },
  {
    ""id"": ""24169acc-e72c-4db5-9140-953a18ab61df"",
    ""quantity"": 817,
    ""notifyWhenLow"": true,
    ""howLow"": 12,
    ""name"": ""fluconazole""
  },
  {
    ""id"": ""4fab3dd6-adb9-45e6-b614-4d8ae1765bbd"",
    ""quantity"": 411,
    ""notifyWhenLow"": false,
    ""howLow"": 2,
    ""name"": ""GLYCOPYRROLATE""
  },
  {
    ""id"": ""ff1e63f8-965d-4b8c-a8e1-88a397887111"",
    ""quantity"": 773,
    ""notifyWhenLow"": false,
    ""howLow"": 15,
    ""name"": ""Ibuprofen""
  },
  {
    ""id"": ""61a00da9-3769-4eb3-b15a-5ea6e462e432"",
    ""quantity"": 39,
    ""notifyWhenLow"": false,
    ""howLow"": 4,
    ""name"": ""Ezetimibe and Simvastatin""
  },
  {
    ""id"": ""790d3909-c24a-4697-b57f-8a754ba95fac"",
    ""quantity"": 691,
    ""notifyWhenLow"": false,
    ""howLow"": 9,
    ""name"": ""Dicyclomine Hydrochloride""
  },
  {
    ""id"": ""e7b96635-b977-43f3-8e74-63a67696d430"",
    ""quantity"": 503,
    ""notifyWhenLow"": true,
    ""howLow"": 16,
    ""name"": ""Amitriptyline Hydrochloride""
  },
  {
    ""id"": ""34bb1a70-9116-4a51-bfaa-4440f4403270"",
    ""quantity"": 77,
    ""notifyWhenLow"": true,
    ""howLow"": 4,
    ""name"": ""Hawaiian Tropic""
  },
  {
    ""id"": ""2b57361d-5dfc-4098-9dfe-74bf2def53bd"",
    ""quantity"": 251,
    ""notifyWhenLow"": false,
    ""howLow"": 16,
    ""name"": ""Argentum nitricum""
  },
  {
    ""id"": ""75517076-f88e-471f-a1ce-eb0883876919"",
    ""quantity"": 115,
    ""notifyWhenLow"": false,
    ""howLow"": 3,
    ""name"": ""esomeprazole magnesium""
  },
  {
    ""id"": ""294a2e9d-cd55-4537-b100-9428c07c70d9"",
    ""quantity"": 622,
    ""notifyWhenLow"": false,
    ""howLow"": 5,
    ""name"": ""Anticavity Fluoride Rinse""
  },
  {
    ""id"": ""cf4ad3cb-2c03-4917-b2d0-eb55da166312"",
    ""quantity"": 919,
    ""notifyWhenLow"": true,
    ""howLow"": 11,
    ""name"": ""Topiramate""
  },
  {
    ""id"": ""c7c41686-94a5-4294-a579-7b20551cdee9"",
    ""quantity"": 302,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""Cyproheptadine Hydrochloride""
  },
  {
    ""id"": ""8d1e73e2-d37d-4e99-b565-22fc609c9985"",
    ""quantity"": 789,
    ""notifyWhenLow"": false,
    ""howLow"": 18,
    ""name"": ""HAND SANITIZER""
  },
  {
    ""id"": ""b1d6a391-c2ba-46a5-81a7-dd179aa5e598"",
    ""quantity"": 327,
    ""notifyWhenLow"": false,
    ""howLow"": 4,
    ""name"": ""Aspirin""
  },
  {
    ""id"": ""bd374050-d4bb-486a-8ac3-309a3da107ed"",
    ""quantity"": 839,
    ""notifyWhenLow"": true,
    ""howLow"": 8,
    ""name"": ""Nitrofurantoin (monohydrate/macrocrystals)""
  },
  {
    ""id"": ""cf3b8e9c-de32-4226-a2d1-42ba649f8fb1"",
    ""quantity"": 30,
    ""notifyWhenLow"": false,
    ""howLow"": 13,
    ""name"": ""SUNSHINE SKIN TINT BROAD SPECTRUM SPF 30 - REFILL""
  },
  {
    ""id"": ""435db28c-b5d6-4ca3-95ff-df90deeb68c7"",
    ""quantity"": 661,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""ATROPINE SULFATE""
  },
  {
    ""id"": ""53faacd8-b355-4b09-9dde-8bf1fee0dda5"",
    ""quantity"": 313,
    ""notifyWhenLow"": false,
    ""howLow"": 19,
    ""name"": ""Azelastine Hydrochloride""
  },
  {
    ""id"": ""d5ca45a0-a7e4-4f8b-8be5-3e890fe39ab3"",
    ""quantity"": 898,
    ""notifyWhenLow"": true,
    ""howLow"": 8,
    ""name"": ""Flumazenil""
  },
  {
    ""id"": ""6798c469-7a4a-460a-b192-d925fae5c0af"",
    ""quantity"": 706,
    ""notifyWhenLow"": false,
    ""howLow"": 6,
    ""name"": ""Pirfenidone""
  },
  {
    ""id"": ""9a4123ec-266d-4d6b-9e2a-3acb83eb1cdf"",
    ""quantity"": 790,
    ""notifyWhenLow"": false,
    ""howLow"": 20,
    ""name"": ""ONDANSETRON""
  },
  {
    ""id"": ""b89c5552-0865-432d-a32c-2d7cd623b82e"",
    ""quantity"": 775,
    ""notifyWhenLow"": true,
    ""howLow"": 11,
    ""name"": ""Ramipril""
  },
  {
    ""id"": ""a1a99042-aa95-4762-804e-16e119c5ee07"",
    ""quantity"": 326,
    ""notifyWhenLow"": true,
    ""howLow"": 3,
    ""name"": ""H.E.B Solutions Sunscreen""
  },
  {
    ""id"": ""7fb1bb53-c3ae-4ff7-89b6-715c01bfe30d"",
    ""quantity"": 634,
    ""notifyWhenLow"": true,
    ""howLow"": 18,
    ""name"": ""Olopatadine Hydrochloride Ophthalmic Solution""
  },
  {
    ""id"": ""a796b05f-13b7-4b79-b735-4e8ccaf09ad4"",
    ""quantity"": 485,
    ""notifyWhenLow"": true,
    ""howLow"": 5,
    ""name"": ""Senna laxative""
  },
  {
    ""id"": ""463735ec-6496-4178-a09f-0eaddf7b092c"",
    ""quantity"": 678,
    ""notifyWhenLow"": false,
    ""howLow"": 12,
    ""name"": ""Hydroxychloroquine Sulfate""
  },
  {
    ""id"": ""1388b3ce-edf8-48a3-944c-6c7a4df1b9d1"",
    ""quantity"": 144,
    ""notifyWhenLow"": true,
    ""howLow"": 14,
    ""name"": ""Undecylenic Acid""
  },
  {
    ""id"": ""1c83dd79-7845-49c5-b028-29862f703d3a"",
    ""quantity"": 286,
    ""notifyWhenLow"": false,
    ""howLow"": 13,
    ""name"": ""Daytime Cold and Flu""
  },
  {
    ""id"": ""bef2625b-c5aa-4a65-a075-6505d6a206da"",
    ""quantity"": 476,
    ""notifyWhenLow"": true,
    ""howLow"": 7,
    ""name"": ""Metoprolol Tartrate""
  },
  {
    ""id"": ""547bf651-3e78-435a-9430-15207c5310c9"",
    ""quantity"": 30,
    ""notifyWhenLow"": false,
    ""howLow"": 17,
    ""name"": ""Citalopram""
  },
  {
    ""id"": ""61f44afd-edbe-43d1-b8d9-0a1f7e60ee44"",
    ""quantity"": 546,
    ""notifyWhenLow"": true,
    ""howLow"": 10,
    ""name"": ""Fexofenadine HCl and Pseudoephedrine HCI""
  },
  {
    ""id"": ""e9903eba-2e22-4b01-a53f-2fd8e29a6eaa"",
    ""quantity"": 288,
    ""notifyWhenLow"": false,
    ""howLow"": 4,
    ""name"": ""Clindamycin""
  }
]

";

    }
}
