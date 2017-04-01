using Bringpro.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        public int? SettingId { get; set; }

        public SettingsCategory CategoryEnum { get; set; }

        public SettingsType SettingTypeEnum { get; set; }

        public SettingsSection SettingSectionEnum { get; set; }

    }
}