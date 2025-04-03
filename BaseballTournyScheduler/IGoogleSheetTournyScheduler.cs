using Google.Apis.Sheets.v4.Data;

namespace BaseballTournyScheduler;

public interface IGoogleSheetTournyScheduler
{
    Spreadsheet GetSpreadsheet(string spreadsheetId);
    
    ValueRange GetSingleValue(string spreadsheetId, string valueRange);
    void RemoveSingleValue(string spreadsheetId, string valueRange);
    BatchGetValuesResponse GetMultipleValues(string spreadsheetId, string[] ranges);
    
}