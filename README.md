# Final Interview for One Acre Fund

## Current Project Status
All the minimum requirements have been met, but the display of the proposed changes before saving has not been attempted as part of this deliverable.

## Estimate on the outstanding work
Given one more day, the display of the proposed changes before saving would have been implemented.

## Successes/what went well
The peer programming meetings were amazing and the more I spoke, the more I got insight into how to carry out this project, I never did it as part of an interview before so it was refreshing and the project has been sufficiently provocative, so I had fun.

## Bumps/what you wished went better
Other than the electoral issues that delayed my work a little, the experience was fine, and didn't face much of an issue.

## How you would improve your approach in future projects
Speak out load even when I am not having a peer programming session with someone else, since I noticed it made my wok faster.

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

