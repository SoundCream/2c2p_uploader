# 2c2p_uploader
Upload XML &amp; CSV Transactions

## Require
.Netcore 3.1 <br />
MS SqlServer 

## Before Run a Project 
1. Create Your database on MS sql server.
2. Run sql script to create Tables & Initdata from \Resources\2C2PInitDatabase.sql
3. Change connectionstring on \2C2P.FileUploader\appsettings.json
4. Run project "2C2P.FileUploader"
5. Files for test on \Resources\test_files
6. Done.

## How to use 
You can see Api Explorer on Menu "Api Explorer"

GET: /api/Transactions

### parameters

<b>currentcy :</b> ISOISO4217 (case insensitive) <i>EX: THB</i> 

<b>statusCode :</b> Allowed "A" or "D" or "R" (case insensitive)  <i>EX: a</i> 
 
<b>fromDate :</b> from date  format yyyyMMdd  <i>EX: 20200220</i> 

<b>toDate :</b> to date format yyyyMMdd  <i>EX: 20200221</i> 
