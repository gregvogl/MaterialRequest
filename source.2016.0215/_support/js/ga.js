  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-1399660-18', 'colostate.edu');
  ga('send', 'pageview');
  
  // track clicks of outbound, mail and file links using Universal Analytics
// Greg Vogl 2014-02-27 
// This code must be executed after the DOM is loaded.
// If jQuery is not used, remove $, append (), and move this code to a <script> tag just before closing </body> tag.
$(function trackClicks() {
    var hitCallbackHandler = function (url, win) {
        if (win) {
            window.open(url, win);
        } else {
            window.location.href = url;
        }
    };
    if (document.getElementsByTagName) {
        // replace with current URL of production application
        var hostRE = new RegExp('(wsnet(dev)?.colostate.edu\/cwis6\/materialrequest)', 'ig');
        var fileRE = new RegExp('\.(pdf|docx?|xlsx?|pptx?|pps|png|gif|jpg|txt|xml|zip|gz|mov|mp4|avi|wmv|wav|mp3|exe|msi|rpm|rss|ics)([?#].*)?$', 'ig');
        var httpRE = '^https?\:\/\/';
        var mailRE = new RegExp('^mailto\:', 'i');
        var telRE = new RegExp('^tel\:', 'i');
        var el = document.getElementsByTagName('a');
        for (var i = 0; i < el.length; i++) {
            var href = (typeof (el[i].getAttribute('href')) == 'string') ? el[i].getAttribute('href') : '';
            if ((href.match(httpRE) && !href.match(hostRE)) || href.match(fileRE) || href.match(mailRE) || href.match(telRE)) {
                el[i].addEventListener('click', function (e) {
                    var url = this.getAttribute('href');
                    var win = (typeof (this.getAttribute('target') == 'string')) ? this.getAttribute('target') : '';
                    var eventType =
                        url.match(httpRE) ? 'Outbound Link' :
                        url.match(fileRE) ? 'Download' :
                        url.match(mailRE) ? 'Mail' :
                        url.match(telRE) ? 'Telephone' :
                        'Error';
                    if (url.match(fileRE))
                        url = url.replace(httpRE, '').replace(hostRE, '');
                    ga('send', 'event', eventType, 'click', url,
						{ 'hitCallback': hitCallbackHandler(url, win) },
						{ 'nonInteraction': 1 }
					);
                    e.preventDefault();
                });
            }
        }
    }
});