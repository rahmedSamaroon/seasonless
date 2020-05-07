# Final Interview for One Acre Fund


## Current Project Status



## Estimate on the outstanding work


## Successes/what went well

## Bumps/what you wished went better

## How you would improve your approach in future projects

## Improvements/enhancements to this project for future consideration
One thing that I would consider is the repayment adjustustments are done. What I woudl suggest is saving the repayment uploads to the database and assoicate each repayment with that upload for example 

If a client starts out with 2 seasons of outstanding credit (debt):

-	CustomerSummary (Client owes 20)
    -	Season = 2011
    -	TotalRepaid = 80
    -	TotalCredit = 100

-	CustomerSummary (Client owes 90)
    -	Season = 2012
    -	TotalRepaid = 30
    -	TotalCredit = 120

When the client makes a payment of 60, we would expect to save 2 repayment records:

-	Repayment record #1 - Season = 2011, Amount = +20 - original repayment record
-	Repayment record #3 - Season = 2012, Amount = +40 - adjustment repayment record

Where each of them are referencing the Id of the upload that has led to these changes.

