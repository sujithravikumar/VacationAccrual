# Vacation Accrual Forecast Calculator

A calculator used for forecasting the vacation accrual for the pay periods. 

## Definitions

### Input Values
* Current Pay Period Startdate (Unit: Date) - Startdate (Sunday) of the current pay period (2 weeks)
* Max Balance Limit (Unit: Hours) - Maximum balance hours limit above which accrual will be forfeited
* Pay Periods (Unit: Number) - Number of pay periods to be displayed for forecasting
* Accrual (Unit: Hours) - Accrual hours per pay period, for example: 6
* Previous Pay Period Balance (Unit: Hours) - Balance hours of previous pay period, for example: 100

### Result Table
* Take days off (Unit: Days) - Take number of days off before it hits the max balance limit (1 day = 8 hours)
* Period - Pay periods (2 weeks interval)
* Accrual - Accrual hours for a given pay period
* Take (Unit: Hours) - Number of hours that you're planning to take off for a given pay period, for example: 8
* Balance - Balance hours for a given pay period (capped at max balance limit)
* Forefeit - Number of hours lost for a given pay period due to balance accrual exceeding the max limit

## URL Arguments for Input Values

Input | Argument | Example
--- | --- | ---
Current Pay Period Startdate | `StartDate` | StartDate=05%2F06%2F18
Max Balance Limit | `MaxBalance` | MaxBalance=120
Pay Periods | `Period` | Period=8
Accrual | `Accrual` | Accrual=6
Previous Pay Period Balance | `Balance` | Balance=100