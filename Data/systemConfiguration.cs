using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class systemConfiguration
{
    public string player_nick_name { get; set; }
    public string web_base { get; set; }
    public string service_base { get; set; }
    public string player_guid { get; set; }
    public string mac { get; set; }
    public string player_email { get; set; }
    public string service_email { get; set; }
    public string warmbug_lock { get; set; }

    public systemConfiguration()
    { }
    public systemConfiguration(string _json)
    {
        systemConfiguration system_configuration = JsonMapper.ToObject<systemConfiguration>(_json);
        this.player_nick_name = system_configuration.player_nick_name;
        this.web_base = system_configuration.web_base;
        this.service_base = system_configuration.service_base;
        this.player_guid = system_configuration.player_guid;
        this.mac = system_configuration.mac;
        this.player_email = system_configuration.player_email;
        this.service_email = system_configuration.service_email;
        this.warmbug_lock = system_configuration.warmbug_lock;
    }
}
