# Material Purchase Request Form for Demand-Driven Acquisitions (DDA)
Morgan Library, Colorado State University

## Demand-Driven Acquisition (DDA) materials 
* Print books
* CSU patrons (current students, faculty and staff) request
* CSU library purchases 

## Workflow Overview
* Library staff load DDA records into the library catalog.
* A CSU patron selects a DDA book and logs in with their CSU eID.
* If the book is available in Prospector, the patron can choose to borrow the book from Prospector.
* Otherwise, the patron can request the book via an online form.
* The request form sends an email to library Collections and Contracts staff. 
* Library staff purchase the book, update the catalog record, and possibly notify the patron that the book is available for checkout.

## Implementation Details

### Software Components
* Sage (CSU library catalog) - Innovative Millennium, WebPAC Pro, Z39.50, to be replaced by Alma in 2016-7
* Prospector (Colorado/regional library catalog) - Innovative Millennium, WebPAC Pro, Encore
* Discovery (CSU Web-scale discovery tool) - VuFind beta (0.8, 2009), Solr 1.4, Java 1.6, PHP, to be replaced by Primo in 2016-7
* WebAuth - ASP.NET C# application/library for login via CSU eID, to be replaced by Shibboleth in 2016
* SQL server databases - CSU eID database with Person table, Request database/table, accessed via Linq to SQL
* Material Purchase Request Form - ASP.NET C# application, created in 2011

### Catalog Integration
* In the MARC record of each DDA book, the 856u field contains the request URL http://librequest.colostate.edu.
* In Discovery display of Sage records, books with a location code of iws (MARC 998a) are given a library location of “Books Purchased on Request”.
* In search results and record views in Sage (via JavaScript) and in Discovery (via JavaScript and Smarty templates), if 856 u is the request URL, a button links to the request form.
* DDA records do not have a bib number in Prospector, so Prospector request links use an ISBN search.
* The request URL is an alias to the server hosting the application, which the application redirects to the actual URL of the production application: https://wsnet.colostate.edu/cwis6/MaterialRequest/

### Application Workflow: Catalog Record
* The referring URL of the record is passed to the form upon login.
* The bib number is extracted from the referring URL using a regular expression. Sage: record=b1234567 Discovery: Record/.b1234567X
* The bib number is looked up in Discovery's Solr database for details, including title, author, publisher and ISBNs. 

### Application Workflow: Prospector Record
* If the book is in Discovery, its first ISBN is used to search classic Prospector.
* If the book is in Prospector, book details, non-CSU copies, and available copies are counted by screen-scraping the holdings rows from the classic Prospector availability page.
* If copies are available, the user can select from links to request from Prospector, view availability in Prospector, or request order for CSU.

### Application Workflow: Order Form
* If no copies are available, or the user selects Order, they see their contact information, requested book information, and delivery options (order, order and notify me when it arrives, or rush order).
* Library staff members can also provide comments or details about the request, such as the identity of a patron making the request.
* If the user clicks Order, a confirmation message is composed. 
* The message is logged to the Material Request database.
* The message sent via email to the patron. (The Collections and Contracts staff email is a BCC recipient.)
* The message is displayed on a confirmation page, with options to print, return to the catalog record, or log out.

# Contact
Greg Vogl, Middleware Developer, Academic Computing and Networking Services,
(970) 491-4394,
Gregory.Vogl@colostate.edu

February 25, 2016
