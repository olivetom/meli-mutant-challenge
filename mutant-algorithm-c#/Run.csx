#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

const double revenuePerkW = 0.12;
const double technicianCost = 250;
const double turbineCost = 100;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    // Get query strings if they exist
    int tempVal;
    int? hours = Int32.TryParse(req.Query["hours"], out tempVal) ? tempVal : (int?)null;
    int? capacity = Int32.TryParse(req.Query["capacity"], out tempVal) ? tempVal : (int?)null;

    // Get request body
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);

    // Use request body if a query was not sent
    capacity = capacity ?? data?.capacity;
    hours = hours ?? data?.hours;

    // Return bad request if capacity or hours are not passed in
    if (capacity == null || hours == null){
        return new BadRequestObjectResult("Please pass capacity and hours on the query string or in the request body");
    }
    // Formulas to calculate revenue and cost
    double? revenueOpportunity = capacity * revenuePerkW * 24;  
    double? costToFix = (hours * technicianCost) +  turbineCost;
    string repairTurbine;

    if (revenueOpportunity > costToFix){
        repairTurbine = "Yes";
    }
    else {
        repairTurbine = "No";
    };

    return (ActionResult)new OkObjectResult(new{
        message = repairTurbine,
        revenueOpportunity = "$"+ revenueOpportunity,
        costToFix = "$"+ costToFix
    });
}