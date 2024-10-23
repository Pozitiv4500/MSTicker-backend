using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SecuritiesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public SecuritiesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{ticker}")]
public async Task<ActionResult<Security>> GetSecurity(string ticker)
{
    var response = await _httpClient.GetStringAsync($"https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}.json");
    var data = JObject.Parse(response);

    var securityData = data["securities"]["data"]
        .FirstOrDefault(security => security[0].ToString() == ticker);
    
    if (securityData == null)
    {
        return NotFound();
    }

    var security = new Security
    {
        SECID = securityData[0]?.ToString(),
        BOARDID = securityData[1]?.ToString(),
        SHORTNAME = securityData[2]?.ToString(),
        PREVPRICE = securityData[3]?.Type == JTokenType.Null ? (double?)null : (double)securityData[3],
        LOTSIZE = securityData[4]?.Type == JTokenType.Null ? (int?)null : (int)securityData[4],
        FACEVALUE = securityData[5]?.Type == JTokenType.Null ? (double?)null : (double)securityData[5],
        STATUS = securityData[6]?.ToString(),
        BOARDNAME = securityData[7]?.ToString(),
        DECIMALS = securityData[8]?.Type == JTokenType.Null ? (int?)null : (int)securityData[8],
        SECNAME = securityData[9]?.ToString(),
        REMARKS = securityData[10]?.ToString(),
        MARKETCODE = securityData[11]?.ToString(),
        INSTRID = securityData[12]?.ToString(),
        SECTORID = securityData[13]?.ToString(),
        MINSTEP = securityData[14]?.Type == JTokenType.Null ? (double?)null : (double)securityData[14],
        PREVWAPRICE = securityData[15]?.Type == JTokenType.Null ? (double?)null : (double)securityData[15],
        FACEUNIT = securityData[16]?.ToString(),
        PREVDATE = securityData[17]?.ToString(),
        ISSUESIZE = securityData[18]?.Type == JTokenType.Null ? (long?)null : (long)securityData[18],
        ISIN = securityData[19]?.ToString(),
        LATNAME = securityData[20]?.ToString(),
        REGNUMBER = securityData[21]?.ToString(),
        PREVLEGALCLOSEPRICE = securityData[22]?.Type == JTokenType.Null ? (double?)null : (double)securityData[22],
        CURRENCYID = securityData[23]?.ToString(),
        SECTYPE = securityData[24]?.ToString(),
        LISTLEVEL = securityData[25]?.Type == JTokenType.Null ? (int?)null : (int)securityData[25],
        SETTLEDATE = securityData[26]?.ToString(),
    };

    return Ok(security);
    }




    [HttpGet("{ticker}/marketdata")]
    public async Task<ActionResult<MarketData>> GetMarketData(string ticker)
    {
        var response = await _httpClient.GetStringAsync($"https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}.json");
        var data = JObject.Parse(response);

        var marketDataArray = data["marketdata"]["data"]
            .FirstOrDefault(market => market[0].ToString() == ticker);
        
        if (marketDataArray == null)
        {
            return NotFound();
        }

        var marketData = new MarketData
        {
            SECID = marketDataArray[0].ToString(),
            BOARDID = marketDataArray[1].ToString(),
            BID = (double?)marketDataArray[2],
            OFFER = (double?)marketDataArray[4],
            OPEN = (double?)marketDataArray[9],
            LOW = (double?)marketDataArray[10],
            HIGH = (double?)marketDataArray[11],
            LAST = (double?)marketDataArray[12],
            QTY = (int)marketDataArray[16],
            VALUE = (double?)marketDataArray[17],
            CLOSEPRICE = (double?)marketDataArray[26],
            CURRENCYID = marketDataArray[27]?.ToString(),
            TIME = (string)marketDataArray[30]
        };

        return Ok(marketData);
    }

    [HttpGet("{ticker}/dataversion")]
    public async Task<ActionResult<DataVersion>> GetDataVersion(string ticker)
    {
        var response = await _httpClient.GetStringAsync($"https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}.json");
        var data = JObject.Parse(response);

        var dataVersionArray = data["dataversion"]["data"].FirstOrDefault();
        
        if (dataVersionArray == null)
        {
            return NotFound();
        }

        var dataVersion = new DataVersion
        {
            DataVersionNumber = (int)dataVersionArray[0],
            SeqNum = (long)dataVersionArray[1]
        };

        return Ok(dataVersion);
    }
}
  