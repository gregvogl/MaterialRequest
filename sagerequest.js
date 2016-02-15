// 
// Add ?record=bibnumber query parameter to Material Request Form links in search results and items
// Used by the Library Catalog (Sage) in screens/botlogo.html 
// Colorado State University Libraries
// Greg Vogl 2011-08-24
//

function sagerequest() {
	// var formURL = "http://purl.access.gpo.gov/GPO/LPS88158";
	// var formURL = "http://wsnetdev.colostate.edu/cwis6/MaterialRequest/";
	var formURL = "http://librequest.colostate.edu/";
	var linkText = "This book is available for the Library to purchase in print. Click here to place a request.";
	$('td.briefCitRow a[href="' + formURL + '"]').each(function(i){
		var bib = $(this).closest('td.briefCitRow').html().match(/value=\"(b\d+)/)[1];
		$(this).attr('href', formURL + '?record=' + bib);
		$(this).text(linkText);
	});
	$('table.bibLinks a[href="' + formURL + '"]').each(function(i){
		var bib = $('td#bibRecordLinkLabel').next('td').html().match(/record=(b\d+)/)[1];
		$(this).attr('href', formURL + '?record=' + bib);
	});
}
sagerequest();
//$(document).ready(sagerequest);

	// Search Results
	
	// bib number: <td width="10%" align="center" class="briefcitEntry" rowspan="2"><div class="briefcitEntryNum"><a name="anchor_2"></a> 2</div><input type="checkbox" name="save" value="b2272618"></td>
	// online access URL: <div class="briefcitActions"><a href="http://purl.access.gpo.gov/GPO/LPS89292" class=" external" title=" (non-Libraries link)"><img src="/screens/media_weblink.gif" alt="Internet" border="0">Access online material</a></div>

	// for each access link,
		// get bib from closest('td.briefCitRow').children('input:checkbox[name=save]').val()
		// append to access link URL

	// Item Record
	
	// bib number: <tr><td id="bibRecordLinkLabel">URL for this item</td><td>http://catalog.library.colostate.edu/record=b2901363~S1</td></tr>
	// online access URL: <table align="center" border="0" cellpadding="1" cellspacing="0" class="bibLinks"><tbody><tr><th>&nbsp;</th></tr><tr align="center"><td><a href="http://purl.access.gpo.gov/GPO/LPS88158">Access online version</a><br>

	// access link is first (only) link in table.bibLinks containing the formURL
		// get bib from next td's contents 
		// append to access link URL

