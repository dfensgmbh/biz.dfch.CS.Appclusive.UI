using biz.dfch.CS.Appclusive.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace biz.dfch.CS.Appclusive.UI.Config
{
    public class NavigationConfig
    {
        static NavigationConfig()
        {
            NavigationConfig.NavEntries = new List<NavEntry>();
            NavigationConfigurationSection configSection = (NavigationConfigurationSection)System.Configuration.ConfigurationManager.GetSection(NavigationConfigurationSection.SectionName);
            foreach (NavEntryElement groupConfig in configSection.NavGroups)
            {
                foreach (NavEntryElement entryConfig in groupConfig.NavEntryElements)
                {
                    NavigationConfig.NavEntries.Add(AutoMapper.Mapper.Map<NavEntry>(entryConfig));
                }
            }
        }
        private static List<NavEntry> NavEntries;


        public static string GetIcon(Type modelType)
        {
            return GetIcon(modelType.Name);
        }
        public static string GetIcon(string modelType)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(modelType));
            string controllerName = modelType.Trim();
            NavEntry modelEntry = NavigationConfig.NavEntries.FirstOrDefault(e => e.Controller == controllerName);
            if (null == modelEntry)
            {
                controllerName += "s";
                modelEntry = NavigationConfig.NavEntries.FirstOrDefault(e => e.Controller == controllerName);
            }
            if (null == modelEntry)
            {
                modelEntry = NavigationConfig.NavEntries.FirstOrDefault(e => e.Name == modelType.Trim());
            }
            if (null != modelEntry)
            {
                return modelEntry.Icon;
            }
            else
            {
                return "";
            }
        }
    }
}