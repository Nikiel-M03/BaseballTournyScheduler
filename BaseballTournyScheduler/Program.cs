using Google.Apis.Auth.OAuth2;
using dotenv.net;
namespace BaseballTournyScheduler
{
    public class TournyScheduler
    {
        public static UserCredential Login(string googleClientId, string googleClientSecret, string[] scopes)
        {
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = googleClientId,
                ClientSecret = googleClientSecret
            };

            return GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None).Result;
            
        }

        static void Main()
        {
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string envPath = Path.Combine(projectRoot, ".env");
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }, ignoreExceptions: false));
            
            string googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID", EnvironmentVariableTarget.Process);
            string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET", EnvironmentVariableTarget.Process);
            string[] scopes = new[] { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets };
            
            UserCredential credential = Login(googleClientId, googleClientSecret, scopes);
            IGoogleSheetTournyScheduler sheetManager = new GoogleSheetTournyScheduler(credential);
            
            var mySpreadSheetId = Environment.GetEnvironmentVariable("SPREADSHEET_ID", EnvironmentVariableTarget.Process);
            var spreadSheet = sheetManager.GetSpreadsheet(mySpreadSheetId);
            
            var teams = sheetManager.GetMultipleValues(mySpreadSheetId, new [] {"A2:A20"});
            foreach (var team in teams.ValueRanges[0].Values)
            {
                Console.WriteLine("Team: " + team[0]);
            }
        }
    }
}

    