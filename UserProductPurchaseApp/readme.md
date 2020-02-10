I have implemented the API for customer login, product viewing, sorting (using oData), purchase
There are 2 test users added to the seed data.
In PostMan, first run the Authenticate/Login endpoint to generate the token. Copy it
Now go to the folder "Product API, Purchase API - requires authorization token (please copy paste token from authentication/login result)" and go to "Authorization" tab.
Enter the copied token and save. This ensures all endpoints within this folder will use this token.
I have already set each endpoint in this folder to use "Inherit from Parent" so the bearer token in step#5 is automatically applied.
Run each endpoint to see the result.
Note: Also attaching a video of execution from postman for your reference.