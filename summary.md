```json
{
  "CreateOpportunity": {
    "records": [
      {
        "Name": "TEST",
        "Description": "This is an example Opportunity",
        "Owner": "João Cardoso",
        "Quote": [
          {
            "Name": "TEST QUOTE",
            "Description": "This is an example Quote",
            "Property1": null,
            "QuoteLine": [
              {
                "Name": "TEST QUOTELINE",
                "Description": "This is an example Quoteline"
              }
            ]
          }
        ]
      }
    ]
  },
  "UpdateOpportunity": {
    "allOrNone": false,
    "records": [
      {
        "attributes": {
          "type": "QuoteLine"
        },
        "Name": "TEST QUOTELINE",
        "Description": "This is an example Quoteline"
      },
      {
        "attributes": {
          "type": "Quote"
        },
        "Name": "TEST QUOTE",
        "Description": "This is an example Quote"
      },
      {
        "attributes": {
          "type": "Opportunity"
        },
        "Name": "TEST",
        "Id": "TEST",
        "Description": "This is an example Opportunity",
        "Owner": "João Cardoso"
      }
    ]
  }
}
