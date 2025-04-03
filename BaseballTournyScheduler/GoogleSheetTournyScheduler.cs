using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ArgumentNullException = System.ArgumentNullException;

namespace BaseballTournyScheduler;

public class GoogleSheetTournyScheduler : IGoogleSheetTournyScheduler
{
    private readonly UserCredential _credential;

    public GoogleSheetTournyScheduler(UserCredential credential)
    {
        _credential = credential;
    }
    public Spreadsheet GetSpreadsheet(string spreadsheetId)
    {
        if (string.IsNullOrEmpty(spreadsheetId))
            throw new ArgumentNullException(nameof(spreadsheetId));

        using (var sheetsService = new SheetsService(new BaseClientService.Initializer()
                   { HttpClientInitializer = _credential }))
        {
            return sheetsService.Spreadsheets.Get(spreadsheetId).Execute();
        }
    }

    public ValueRange GetSingleValue(string spreadsheetId, string valueRange)
    {
        if (string.IsNullOrEmpty(spreadsheetId))
            throw new ArgumentNullException(nameof(spreadsheetId));
        if (string.IsNullOrEmpty(valueRange))
            throw new ArgumentNullException(nameof(valueRange));

        using (var sheetsService = new SheetsService(new BaseClientService.Initializer()
                   { HttpClientInitializer = _credential }))
        {
            return sheetsService.Spreadsheets.Values.Get(spreadsheetId, valueRange).Execute();
        }
    }

    public void RemoveSingleValue(string spreadsheetId, string valueRange)
    {
        if (string.IsNullOrEmpty(spreadsheetId))
            throw new ArgumentNullException(nameof(spreadsheetId));
        if (string.IsNullOrEmpty(valueRange))
            throw new ArgumentNullException(nameof(valueRange));

        using (var sheetsService = new SheetsService(new BaseClientService.Initializer()
                   { HttpClientInitializer = _credential }))
        {
            sheetsService.Spreadsheets.Values.Clear(new ClearValuesRequest(), spreadsheetId, valueRange).Execute();
        }
    }

    public BatchGetValuesResponse GetMultipleValues(string spreadsheetId, string[] ranges)
    {
        if (string.IsNullOrEmpty(spreadsheetId))
            throw new ArgumentNullException(nameof(spreadsheetId));
        if (ranges == null || ranges.Length == 0)
            throw new ArgumentNullException(nameof(ranges));

        using (var sheetsService = new SheetsService(new BaseClientService.Initializer()
                   { HttpClientInitializer = _credential }))
        {
            var getValueRequest = sheetsService.Spreadsheets.Values.BatchGet(spreadsheetId);
            getValueRequest.Ranges = ranges;
            return getValueRequest.Execute();
        }
    }
}