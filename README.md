# Material Purchase Request Form for Demand-Driven Acquisitions (DDA)
Greg Vogl, Colorado State University Libraries, 2015-02-15

Demand-Driven Acquisitions (DDA) materials are mostly books and print items which the CSU library purchases when CSU patron request them.

## Workflow Overview:
* Library staff load DDA records into the library catalog.
* A CSU patron selects a DDA item and logs in with their CSU eID.
* If the item is available in Prospector, the patron can choose to borrow the item from Prospector.
* Otherwise, the patron can request the item via an online form.
* The request form sends an email to library Collections and Contracts staff. 
* Library staff purchase the item and update the catalog record.

## Implementation Details:

### Software Components:
* Sage (CSU library catalog, Innovative Millennium, WebPAC Pro, to be replaced by Alma in 2016-7)
* Prospector (regional library catalog)
* Discovery (Web-scale discovery tool, customized VuFind beta (0.8, 2009), running Solr 1.4, Java 1.6, PHP, to be replaced by Primo in 2016-7)
* Material Purchase Request Form (ASP.NET C# application created in 2011)
* SQL server databases (accessed using Linq to SQL): CSU eID database with Person table, and Request database with Request table
* WebAuth (ASP.NET C# application and library for CSU login via eID, to be replaced by Shibboleth in 2016)

### Catalog Integration: 
* In the MARC record of each DDA item, the 856u field contains the request URL http://librequest.colostate.edu.
* In Discovery indexing of Sage records, items with type are given a library location of "Books Purchased Upon request"
* In search results and record views in Sage (via JavaScript) and in Discovery (via JavaScript and Smarty templates), if 856 u is the request URL, a button links to the request form.
* DDA items do not have a bib number in Prospector, so Prospector request links use an ISBN search.
* The request URL is an alias to the server hosting the application, which the application redirects to the actual URL of the production application: https://wsnet.colostate.edu/cwis6/MaterialRequest/

### Application Workflow:
* The referring URL of the record is passed to the form upon login.
* The bib number is extracted from the referring URL using a regular expression. Sage: record=b1234567 Discovery: Record/.b1234567X
* The bib number is looked up in Discovery's Solr database for details, including title, author, publisher and ISBNs. 
* If the item is in Discovery, its first ISBN is used to search classic Prospector.
* If the item is in Prospector, item details, non-CSU and available copies are counted by screen-scraping the holdings row from the classic Prospector availability page.
* If copies are available, the user can select from links to request from Prospector, view availability in Prospector, or request order for CSU.
* If no copies are available, or the user selects Order, they see their contact information, requested item information, and delivery options.
* If the user is a library staff member, they can also provide details about the request, such as the identity of a patron making the request.
* If the user clicks Order, a confirmation message is composed. 
* The message is logged to the Material Request database.
* The message sent via email to the patron. (The Collections and Contracts staff email is a BCC recipient.)
* The message is displayed on a confirmation page, with options to print, return to the catalog item, or log out.
