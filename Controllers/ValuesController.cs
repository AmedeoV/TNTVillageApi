    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;
    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Mvc;

    namespace TNTVillageApi.Controllers {
        [Route ("api/[controller]")]
        public class ValuesController : Controller {
            private static readonly HttpClient client = new HttpClient ();
            // GET api/values
            [HttpGet]
            public IEnumerable<string> Get () {
                return new string[] { "value1", "value2" };
            }

            // GET api/values/5
            [HttpGet ("{searchTerm}")]
            public async Task<string> GetAsync (string searchTerm) {

                var xxx = await DoPostAsync (searchTerm);

                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument ();
                htmlDocument.LoadHtml (xxx);
                var tot_pag_addr = htmlDocument.DocumentNode.SelectSingleNode ("//div[@class='pagination']/form/span/b[3]/text()").InnerHtml;
                var result_table_addr = htmlDocument.DocumentNode.SelectSingleNode ("//div[@class='showrelease_tb']/table/tr").InnerHtml;
                var title_addr = htmlDocument.DocumentNode.SelectSingleNode ("./td[7]/a/text()");
                var desc_addr = htmlDocument.DocumentNode.SelectSingleNode ("./td[7]/text()");
                var leech_addr = htmlDocument.DocumentNode.SelectSingleNode ("./td[4]/text()");
                var seed_addr = htmlDocument.DocumentNode.SelectSingleNode ("./td[5]/text()");
                // var dl_addr = htmlDocument.DocumentNode.SelectSingleNode ("//div[@class='showrelease_tb']/table/tr[{}]/td[1]/a/@href");

                // tot_pag_addr = "//div[@class='pagination']/form/span/b[3]/text()"
                // result_table_addr = "//div[@class='showrelease_tb']/table/tr"
                // title_addr = "./td[7]/a/text()"
                // desc_addr = "./td[7]/text()"
                // leech_addr = "./td[4]/text()"
                // seed_addr = "./td[5]/text()"
                // dl_addr = "//div[@class='showrelease_tb']/table/tr[{}]/td[1]/a/@href"

                return tot_pag_addr.ToString();
            }

            // POST api/values
            [HttpPost]
            public void Post ([FromBody] string value) { }

            public async Task<string> DoPostAsync (string searchTerm) {

                var values = new Dictionary<string, string> { { "srcrel", searchTerm },
                        { "page", "1" },
                        { "cat", "0" }
                    };
                var content = new FormUrlEncodedContent (values);

                var response = await client.PostAsync ("http://www.tntvillage.scambioetico.org/src/releaselist.php", content);

                var responseString = await response.Content.ReadAsStringAsync ();

                return responseString;
            }

            // PUT api/values/5
            [HttpPut ("{id}")]
            public void Put (int id, [FromBody] string value) { }

            // DELETE api/values/5
            [HttpDelete ("{id}")]
            public void Delete (int id) { }
        }
    }