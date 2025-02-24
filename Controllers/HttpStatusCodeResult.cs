using System.Net;
using Microsoft.AspNetCore.Mvc;

internal class HttpStatusCodeResult : ActionResult
{
    private HttpStatusCode badRequest;
    private string v;

    public HttpStatusCodeResult(HttpStatusCode badRequest, string v)
    {
        this.badRequest = badRequest;
        this.v = v;
    }
}