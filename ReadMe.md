# API Integration Technical Test

We need to connect a new provider (id = 1) and download their JSON tour availability data via an API
call, manipulate it into what we need, and then import it into our database.Please create a fork using your github account and create a branch named after your name for your code.When you're happy and have completed your tasks please create a pull request and let us know you've finished.The program needs to:

1. Compile without errors
2. Run end to end (ie: complete the import process successfully)
3. Import the availability data for any existing tours
4. Skip any availability data where no tour exists in our database without crashing the program
5. Prices must be adjusted correctly to reflect our discount strategy detailed below
6. Any errors must be logged

You can use any online resource to help you complete the test. We estimate it will take under 2 hours to
complete the test, but there is no actual time limit.Some notes on code setup:
We have 2 projects, a class library for your code - ApiIntegration.csproj and ApiIntegration.Tests for
any test you wish to write. You may alter the projects / create new ones as you see fit to improve coding standards. 

The program entry is in the Importer class, and the Execute function starts the
importer process. The interface ### Solution structure:
- "ProviderModels" holds the providers specific model classes (for deserialization)
- "Models" holds our own models
- "Interfaces" holds the interfaces for our implementations. Feel free to create new ones
or modify the existing ones should you see fit.## Tasks:

In the Importer class, complete the Importer.Begin method so that:
1. **Provider Data Download:**
    - Please implement the IApiDownloader interface. This should take the
    data held at http://tap.techtest.s3-website.eu-west-2.amazonaws.com/ and
    deserialze it into the models stored in the ProviderModels folder.

2. **Data Transformation:**
    - The API response availability data should be parsed into our own TourAvailability model, ready to
      be inserted into the database.
    
3. **Data Manipulation:**
    - The selling price we store in our database is the value we show to customers on the website. This means it needs
    to include our commission on top of the provider price. The commission percentage is found on the provider model
    and is applied to the full provider price.
    - We're also running a sale on the website. The discount is 5% off from the provider's price.
    - SellingPrice = ProviderPrice + Commission - Discount

### Other Notes
- We've implemented in memory versions of the repositories. Please use these implementations rather than setting up a database.
You are welcome to add any new methods to the interfaces should you require them.
- Refactor as much as you like! We're interested to see how you code.