using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development.DataAsClass
{
    internal static class IndividualPatientData
    {
        public static string JsonData = @"

        [
  {
    ""id"": ""6eb50daf-356a-4abd-ba94-7eb901206e2a"",
    ""firstName"": ""Crawford"",
    ""lastName"": ""Kramer"",
    ""otherName"": ""Nona"",
    ""email"": ""nonakramer@optique.com"",
    ""address"": ""606 Dictum Court, Shrewsbury, North Carolina, 4380""
  },
  {
    ""id"": ""b27d5ae9-f961-4e65-afea-429e491a5eb2"",
    ""firstName"": ""Lila"",
    ""lastName"": ""Crane"",
    ""otherName"": ""Rachael"",
    ""email"": ""rachaelcrane@optique.com"",
    ""address"": ""942 Metrotech Courtr, Bainbridge, Illinois, 9410""
  },
  {
    ""id"": ""841a133b-28ec-4042-af45-8fb5111001d7"",
    ""firstName"": ""Simone"",
    ""lastName"": ""Hodges"",
    ""otherName"": ""Henson"",
    ""email"": ""hensonhodges@optique.com"",
    ""address"": ""816 Williamsburg Street, Taycheedah, Tennessee, 4593""
  },
  {
    ""id"": ""6a644814-5636-4e9e-b853-bff4338b60bd"",
    ""firstName"": ""Clarice"",
    ""lastName"": ""William"",
    ""otherName"": ""Ratliff"",
    ""email"": ""ratliffwilliam@optique.com"",
    ""address"": ""199 Applegate Court, Crisman, Hawaii, 5162""
  },
  {
    ""id"": ""434540be-452d-437f-8fbb-2013f227abab"",
    ""firstName"": ""Adeline"",
    ""lastName"": ""Landry"",
    ""otherName"": ""Erna"",
    ""email"": ""ernalandry@optique.com"",
    ""address"": ""609 Eagle Street, Crenshaw, Florida, 8700""
  },
  {
    ""id"": ""83fd3f8a-ef25-4044-bee0-2e662ed17be2"",
    ""firstName"": ""Church"",
    ""lastName"": ""Farrell"",
    ""otherName"": ""Velez"",
    ""email"": ""velezfarrell@optique.com"",
    ""address"": ""699 Hale Avenue, Chesapeake, Wisconsin, 8997""
  },
  {
    ""id"": ""8423e379-2b42-4306-928b-05d14c1b4183"",
    ""firstName"": ""Ashley"",
    ""lastName"": ""Mcdonald"",
    ""otherName"": ""Dixie"",
    ""email"": ""dixiemcdonald@optique.com"",
    ""address"": ""222 Sharon Street, Lewis, Palau, 4982""
  },
  {
    ""id"": ""4c05db22-37b1-4ef1-8ced-2e5f9156613a"",
    ""firstName"": ""Roberta"",
    ""lastName"": ""Hahn"",
    ""otherName"": ""Barber"",
    ""email"": ""barberhahn@optique.com"",
    ""address"": ""149 Kay Court, Waumandee, Kentucky, 4156""
  },
  {
    ""id"": ""bf08bbae-e883-4873-a037-9c76e7fb4111"",
    ""firstName"": ""Kent"",
    ""lastName"": ""Noble"",
    ""otherName"": ""Helen"",
    ""email"": ""helennoble@optique.com"",
    ""address"": ""224 Hendrickson Street, Witmer, Kansas, 1773""
  },
  {
    ""id"": ""dff0558d-25fd-4837-a4a1-f7268aee5161"",
    ""firstName"": ""Emilia"",
    ""lastName"": ""Singleton"",
    ""otherName"": ""Ball"",
    ""email"": ""ballsingleton@optique.com"",
    ""address"": ""591 Royce Place, Sharon, Oregon, 447""
  },
  {
    ""id"": ""bb5209a6-179b-40b3-ac54-d43534582eaf"",
    ""firstName"": ""Concepcion"",
    ""lastName"": ""Bentley"",
    ""otherName"": ""Trudy"",
    ""email"": ""trudybentley@optique.com"",
    ""address"": ""701 Menahan Street, Jacksonwald, New Mexico, 4444""
  },
  {
    ""id"": ""0b35c24e-0e0b-46cb-be03-a23c2de729eb"",
    ""firstName"": ""Oliver"",
    ""lastName"": ""Butler"",
    ""otherName"": ""Solis"",
    ""email"": ""solisbutler@optique.com"",
    ""address"": ""398 Strickland Avenue, Lowell, Puerto Rico, 9615""
  },
  {
    ""id"": ""05f49e4c-c682-406f-98d8-1ecaa3a53b96"",
    ""firstName"": ""Rose"",
    ""lastName"": ""Mcknight"",
    ""otherName"": ""Mara"",
    ""email"": ""maramcknight@optique.com"",
    ""address"": ""207 Argyle Road, Leroy, Wyoming, 3621""
  },
  {
    ""id"": ""1a585207-e84d-444c-8aeb-8ae39c786f08"",
    ""firstName"": ""Lynnette"",
    ""lastName"": ""Dillard"",
    ""otherName"": ""Bernadine"",
    ""email"": ""bernadinedillard@optique.com"",
    ""address"": ""899 Ryerson Street, Tetherow, Guam, 6351""
  },
  {
    ""id"": ""6c597a00-fc66-4759-803e-94a56acfe049"",
    ""firstName"": ""Mia"",
    ""lastName"": ""Livingston"",
    ""otherName"": ""Lilian"",
    ""email"": ""lilianlivingston@optique.com"",
    ""address"": ""400 Sutton Street, Finzel, South Dakota, 6971""
  },
  {
    ""id"": ""1ca6c2fd-7291-481e-9b78-efa1e567358c"",
    ""firstName"": ""Miles"",
    ""lastName"": ""Swanson"",
    ""otherName"": ""Tara"",
    ""email"": ""taraswanson@optique.com"",
    ""address"": ""289 Railroad Avenue, Caroleen, Rhode Island, 9824""
  },
  {
    ""id"": ""564f088e-9204-4a1c-a665-d4cf1f41b51e"",
    ""firstName"": ""Watson"",
    ""lastName"": ""Merritt"",
    ""otherName"": ""Ramona"",
    ""email"": ""ramonamerritt@optique.com"",
    ""address"": ""837 Gunnison Court, Garfield, Oklahoma, 8599""
  },
  {
    ""id"": ""8c6fc912-fbd1-432d-8d22-5a5e50cb2ec7"",
    ""firstName"": ""Mcmahon"",
    ""lastName"": ""Chan"",
    ""otherName"": ""Noble"",
    ""email"": ""noblechan@optique.com"",
    ""address"": ""117 Lefferts Avenue, Watrous, West Virginia, 6665""
  },
  {
    ""id"": ""2a32b3e1-af81-4d87-ae4a-c5764e9940ac"",
    ""firstName"": ""Dominique"",
    ""lastName"": ""Hale"",
    ""otherName"": ""Forbes"",
    ""email"": ""forbeshale@optique.com"",
    ""address"": ""375 Waldorf Court, Clay, Alabama, 4380""
  },
  {
    ""id"": ""f5c132c4-568b-47d7-823a-d62a86013547"",
    ""firstName"": ""Wells"",
    ""lastName"": ""Haynes"",
    ""otherName"": ""Hoover"",
    ""email"": ""hooverhaynes@optique.com"",
    ""address"": ""878 Robert Street, Teasdale, Delaware, 8829""
  },
  {
    ""id"": ""47328df5-111d-4769-8dfe-56e8f7a93fef"",
    ""firstName"": ""Cathryn"",
    ""lastName"": ""Gallagher"",
    ""otherName"": ""Angie"",
    ""email"": ""angiegallagher@optique.com"",
    ""address"": ""106 Denton Place, Kipp, District Of Columbia, 6143""
  },
  {
    ""id"": ""db81c9c6-4f4b-40f7-a29b-b69a866ab6ee"",
    ""firstName"": ""Weaver"",
    ""lastName"": ""Simmons"",
    ""otherName"": ""Hart"",
    ""email"": ""hartsimmons@optique.com"",
    ""address"": ""800 Chester Street, Neahkahnie, Nebraska, 9246""
  },
  {
    ""id"": ""6ff238dd-28bc-4266-9931-067b1bf29e6e"",
    ""firstName"": ""Castillo"",
    ""lastName"": ""Roy"",
    ""otherName"": ""Lupe"",
    ""email"": ""luperoy@optique.com"",
    ""address"": ""217 Lincoln Terrace, Bergoo, Marshall Islands, 7792""
  },
  {
    ""id"": ""b7e2eee1-21de-4ad6-8a6f-6d0041b65ae9"",
    ""firstName"": ""Mattie"",
    ""lastName"": ""Mcclure"",
    ""otherName"": ""Valentine"",
    ""email"": ""valentinemcclure@optique.com"",
    ""address"": ""103 Boerum Street, Kaka, Connecticut, 836""
  },
  {
    ""id"": ""b177ddc4-4470-4064-8f14-ac850cadbb8b"",
    ""firstName"": ""Deirdre"",
    ""lastName"": ""Winters"",
    ""otherName"": ""Melissa"",
    ""email"": ""melissawinters@optique.com"",
    ""address"": ""789 Clark Street, Craig, Massachusetts, 7985""
  },
  {
    ""id"": ""9654521d-6121-48bb-8de1-123a70535c52"",
    ""firstName"": ""Reyes"",
    ""lastName"": ""Alford"",
    ""otherName"": ""Natasha"",
    ""email"": ""natashaalford@optique.com"",
    ""address"": ""993 Duffield Street, Leming, North Dakota, 468""
  },
  {
    ""id"": ""49afb05d-0f40-460c-b8e2-76fab0bf5666"",
    ""firstName"": ""Debra"",
    ""lastName"": ""Deleon"",
    ""otherName"": ""Santiago"",
    ""email"": ""santiagodeleon@optique.com"",
    ""address"": ""335 Taaffe Place, Dante, Northern Mariana Islands, 7791""
  },
  {
    ""id"": ""dab05379-73be-43cd-957f-c62cd2c2162e"",
    ""firstName"": ""Mcknight"",
    ""lastName"": ""Hensley"",
    ""otherName"": ""Tracie"",
    ""email"": ""traciehensley@optique.com"",
    ""address"": ""220 Clermont Avenue, Sutton, Washington, 8856""
  },
  {
    ""id"": ""6a336c16-5a50-4fd8-a5a7-a2c4fbda31f6"",
    ""firstName"": ""Pate"",
    ""lastName"": ""Dudley"",
    ""otherName"": ""Charlene"",
    ""email"": ""charlenedudley@optique.com"",
    ""address"": ""689 Norwood Avenue, Odessa, Virgin Islands, 7302""
  },
  {
    ""id"": ""ce64bf9a-3a96-43a9-925c-2e9fcee86f94"",
    ""firstName"": ""Hewitt"",
    ""lastName"": ""Terrell"",
    ""otherName"": ""Valerie"",
    ""email"": ""valerieterrell@optique.com"",
    ""address"": ""881 Madison Place, Saddlebrooke, Georgia, 8273""
  },
  {
    ""id"": ""2d51382a-d6c9-4625-9259-c02b45c60766"",
    ""firstName"": ""Carter"",
    ""lastName"": ""Payne"",
    ""otherName"": ""Hicks"",
    ""email"": ""hickspayne@optique.com"",
    ""address"": ""352 Harrison Avenue, Cecilia, Federated States Of Micronesia, 1773""
  },
  {
    ""id"": ""447c7790-3258-4a36-b51d-0ce6673ffbbf"",
    ""firstName"": ""Sheila"",
    ""lastName"": ""Gray"",
    ""otherName"": ""Emerson"",
    ""email"": ""emersongray@optique.com"",
    ""address"": ""270 Kings Hwy, Clara, Utah, 3587""
  },
  {
    ""id"": ""a3036f10-922e-4631-b10e-463290a3c6ea"",
    ""firstName"": ""Pickett"",
    ""lastName"": ""Harrell"",
    ""otherName"": ""Tania"",
    ""email"": ""taniaharrell@optique.com"",
    ""address"": ""805 Sullivan Street, Sunwest, Indiana, 7507""
  },
  {
    ""id"": ""126bb587-a85f-4e1b-8913-83e069ce158e"",
    ""firstName"": ""Bertie"",
    ""lastName"": ""Mcpherson"",
    ""otherName"": ""Leah"",
    ""email"": ""leahmcpherson@optique.com"",
    ""address"": ""505 Franklin Avenue, Leeper, Montana, 6087""
  },
  {
    ""id"": ""17945d50-7e25-4607-8ecb-d498a41f104a"",
    ""firstName"": ""April"",
    ""lastName"": ""Parks"",
    ""otherName"": ""Dorothea"",
    ""email"": ""dorotheaparks@optique.com"",
    ""address"": ""493 Bank Street, Talpa, Michigan, 4999""
  },
  {
    ""id"": ""ace278af-7451-4766-8285-f2f297a9d791"",
    ""firstName"": ""Oconnor"",
    ""lastName"": ""Myers"",
    ""otherName"": ""Latisha"",
    ""email"": ""latishamyers@optique.com"",
    ""address"": ""290 Woodside Avenue, Chamberino, Maine, 1386""
  },
  {
    ""id"": ""0ecdec17-8f64-44ec-921e-d7f3bd7bf4d4"",
    ""firstName"": ""Whitfield"",
    ""lastName"": ""Craft"",
    ""otherName"": ""Jewel"",
    ""email"": ""jewelcraft@optique.com"",
    ""address"": ""718 Judge Street, Warsaw, Ohio, 5790""
  },
  {
    ""id"": ""d0c73e88-cc8d-4169-89d6-ae7c122626c1"",
    ""firstName"": ""Mooney"",
    ""lastName"": ""Marshall"",
    ""otherName"": ""Cherry"",
    ""email"": ""cherrymarshall@optique.com"",
    ""address"": ""266 Laurel Avenue, Zeba, American Samoa, 326""
  },
  {
    ""id"": ""8b9a2f5e-aefb-4f59-90a3-9d092ff43ef4"",
    ""firstName"": ""Sara"",
    ""lastName"": ""Hooper"",
    ""otherName"": ""Delacruz"",
    ""email"": ""delacruzhooper@optique.com"",
    ""address"": ""533 River Street, Brethren, Nevada, 5711""
  },
  {
    ""id"": ""6c9d05bc-bbde-4414-b8d8-12b6c06d3b69"",
    ""firstName"": ""Manning"",
    ""lastName"": ""Sutton"",
    ""otherName"": ""Roy"",
    ""email"": ""roysutton@optique.com"",
    ""address"": ""640 Vermont Street, Katonah, Louisiana, 1474""
  },
  {
    ""id"": ""ece52dc0-97ec-4e5c-8e19-0c4895005e07"",
    ""firstName"": ""Humphrey"",
    ""lastName"": ""Morse"",
    ""otherName"": ""Georgette"",
    ""email"": ""georgettemorse@optique.com"",
    ""address"": ""750 Hampton Place, Glenville, Texas, 5314""
  },
  {
    ""id"": ""afa3286e-c2d3-4863-829c-c1dad7430db4"",
    ""firstName"": ""Lakisha"",
    ""lastName"": ""Rodriguez"",
    ""otherName"": ""Shelton"",
    ""email"": ""sheltonrodriguez@optique.com"",
    ""address"": ""548 Kosciusko Street, Snelling, South Carolina, 8413""
  },
  {
    ""id"": ""c29d5140-bd13-40f0-aa03-238235250776"",
    ""firstName"": ""Gordon"",
    ""lastName"": ""Sargent"",
    ""otherName"": ""Head"",
    ""email"": ""headsargent@optique.com"",
    ""address"": ""512 Glen Street, Gardners, New Jersey, 9262""
  },
  {
    ""id"": ""b6a4dc38-81df-48d1-895e-c15e6e60b4fc"",
    ""firstName"": ""Dolores"",
    ""lastName"": ""Ashley"",
    ""otherName"": ""Bonita"",
    ""email"": ""bonitaashley@optique.com"",
    ""address"": ""102 Ocean Avenue, Malo, Vermont, 9130""
  },
  {
    ""id"": ""1c157ad2-0d2a-4418-9a31-72fd0cb97531"",
    ""firstName"": ""Harmon"",
    ""lastName"": ""Barrera"",
    ""otherName"": ""Bettye"",
    ""email"": ""bettyebarrera@optique.com"",
    ""address"": ""663 Hendrix Street, Farmington, California, 9671""
  },
  {
    ""id"": ""a14cb08f-0bc4-4e03-8083-d3663e40b582"",
    ""firstName"": ""Eve"",
    ""lastName"": ""Reed"",
    ""otherName"": ""Casandra"",
    ""email"": ""casandrareed@optique.com"",
    ""address"": ""137 Malbone Street, Gilmore, Missouri, 900""
  }
]
                
        ";
    }
}
