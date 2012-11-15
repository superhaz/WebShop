using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Specialized;
using System.Collections;

namespace WebShop.BLL.Security
{
    /// <summary>
    /// Summary description for QueryStringHelper
    /// this code was reused from:
    /// http://www.csharper.net/blog/querystring_class_useful_for_querystring_manipulation__appendage__etc.aspx
    /// </summary>

    /// <summary>
    /// Title: QueryStringHelper 
    /// Description: Simplifies manipulation of querystrings.
    /// Author: http://www.csharper.net/blog/querystring_class_useful_for_querystring_manipulation__appendage__etc.aspx
    /// Version: 1.0
    /// </summary>
    public class QueryStringHelper : NameValueCollection
    {
        private string document;
    /// <summary>
    /// gets and sets a document
    /// </summary>
    public string Document
    {
        get
        {
            return document;
        }
    }

    public QueryStringHelper()
    {
    }

    public QueryStringHelper(NameValueCollection clone)
        : base(clone)
    {
    }

    public static QueryStringHelper FromCurrent()
    {
        return FromUrl(HttpContext.Current.Request.Url.AbsoluteUri);
    }

    public static QueryStringHelper FromUrl(string url)
    {
        string[] parts = url.Split("?".ToCharArray());
        QueryStringHelper qs = new QueryStringHelper();
        qs.document = parts[0];

        if (parts.Length == 1)
            return qs;

        string[] keys = parts[1].Split("&".ToCharArray());
        foreach (string key in keys)
        {
            string[] part = key.Split("=".ToCharArray());
            if (part.Length == 1)
                qs.Add(part[0], "");
            else
                qs.Add(part[0], part[1]);
        }

        return qs;
    }

    public void ClearAllExcept(string except)
    {
        ClearAllExcept(new string[] { except });
    }

    public void ClearAllExcept(string[] except)
    {
        ArrayList toRemove = new ArrayList();
        foreach (string s in this.AllKeys)
        {
            foreach (string e in except)
            {
                if (s.ToLower() == e.ToLower())
                    if (!toRemove.Contains(s))
                        toRemove.Add(s);
            }
        }

        foreach (string s in toRemove)
            this.Remove(s);
    }

    public override void Add(string name, string value)
    {
        if (this[name] != null)
            this[name] = value;
        else
            base.Add(name, value);
    }

    public override string ToString()
    {
        return ToString(false);
    }

    public string ToString(bool includeUrl)
    {
        string[] parts = new string[this.Count];
        string[] keys = this.AllKeys;
        for (int i = 0; i < keys.Length; i++)
            parts[i] = keys[i] + "=" + this[keys[i]];//+ HttpContext.Current.Server.UrlEncode(this[keys[i]]);
        string url = String.Join("&", parts);
        if ((url != null || url != String.Empty) && !url.StartsWith("?"))
            url = "?" + url;
        if (includeUrl)
            url = this.document + url;
        return url;
    }
    }
}
