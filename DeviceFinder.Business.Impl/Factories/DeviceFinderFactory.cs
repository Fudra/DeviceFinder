using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DeviceFinder.Business.Impl.Models;
using DeviceFinder.Business.Interface.Factories;
using DeviceFinder.Business.Interface.Models;
using DeviceFinder.Data.Interface.Dao;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Business.Impl.Factories
{
    public class DeviceFinderFactory : IDeviceFinderFactory
    {
        #region Fields

        private readonly IDataAccessObject _dao;
        private readonly ISetting _setting;
        private ITranslation _translation;
        private readonly Dictionary<int,string> _propertyNameDictionary = new Dictionary<int, string>();
        private Dictionary<int,string> _propertyGroupDictionary = new Dictionary<int, string>(); 


        #endregion

        #region 

        public DeviceFinderFactory(IDataAccessObject dataAccessObject, ISetting setting)
        {
            _dao = dataAccessObject;
            _setting = setting;
            Init();
        }

        /// <summary>
        /// initiaziliert die DeviceFinderFactroy
        /// </summary>
        private void Init()
        {
            _dao.Init(_setting.Filename);
        }

        /// <summary>
        /// Gibt alle Models aus der Exceltabelle Wieder
        /// </summary>
        /// <returns>Aufzählung aller Models </returns>
        public IEnumerable<IModel> GetAllModelsFromExcelSheet()
        {
            throw new NotSupportedException();
            //var vos =_dao.GetAllExcelModelVos().ToArray();
            //return vos.Select(i => new Model()
            //{
            //    Matnr = i.Matnr,
            //    Coriolis = i.Coriolis,
            //    Gas = i.Gas,
            //    Liquide = i.Liquide,
            //    Magnetic = i.Magnetic,
            //    Pressure = i.Pressure,
            //    Rotameter = i.Rotameter,
            //    Ultrasonic = i.Ultrasonic,
            //    Steam = i.Steam,
            //    Vortex = i.Vortex
            //});
        }

        /// <summary>
        /// Liest alle Produkt Lines aus der Excel Datei aus
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>Aufzählung Product Lines</returns>
        public IEnumerable<IProductLine> GetProductLinesExcel(ISetting setting)
        {
            // reset translation Object
            _translation = null;
            //var views = GetViews();
            //var phaseTypeVisibility = views.First(i => i.FieldName.Contains("Phase"));
            //var technologyVisubility = views.First(i => i.FieldName.Contains("Tech"));
            var pl = _dao.GetAllProductLinesVo(setting.ProductLineSheet);
           
            //var result =  pl.Select(i => new ProductLine()
            //{
            //    Name = i.Name,
            //    PhaseType = new PhaseType()
            //    {
            //        Gas = i.PhaseType.Gas,
            //        Liquide = i.PhaseType.Liquide,
            //        Steam = i.PhaseType.Steam,
            //        Visible = phaseTypeVisibility.Visible
            //    },
            //    Technology = new Technology()
            //    {
            //        Rotameter = i.Technology.Rotameter,
            //        Ultrasonic = i.Technology.Ultrasonic,
            //        Magnetic = i.Technology.Magnetic,
            //        Pressure = i.Technology.Pressure,
            //        Coriolis = i.Technology.Coriolis,
            //        Vortex = i.Technology.Vortex,
            //        Visibility = technologyVisubility.Visible
            //    }
            //}).ToArray();

            var result = pl.Select(i => new ProductLine
            {
                Name = i.Name,
                ProductLineProperties = i.ProductLineProperties.Select(j => new ProductLineProperty
                {
                    Name = GetTranslation(j.Name, setting),
                    Value = j.Value,
                    Group = GetTranslation(j.Group, setting)
                })
            });

            return result;
        }

        /// <summary>
        /// Gibt alle ProductLines aus der DB wieder 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>product lines aufzählung</returns>
        public IEnumerable<IProductLine> GetProductLinesDb(ISetting setting)
        {
            var trasnlation = GetTranslationsDb().Where(i => i.Language.Contains(setting.LanguageDB)).ToArray();

            var productLines = _dao.GetCommonProductLineVos().Select(i => new ProductLine
            {
                Id = i.ProductLineId,
                Name = GetTranslation(i.ProductLineName, trasnlation),
                ProductLineProperties = GetProductLinePropertysDb(i.ProductLineId,trasnlation,setting)
            });

            foreach (var productLine in productLines)
            {
                Debug.WriteLine(productLine.ToString());
            }
            return productLines;
        }

        /// <summary>
        ///   Gibt eien Liste aller Übersetzungen wieder aus Excel 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>Unsertützte Sprachen</returns>
        public IEnumerable<string> GetSupportedTranslationsExcel(ISetting setting)
        {
           return GetTranslationsExcel(setting).Select(i=>i.Language);
        }

        /// <summary>
        ///   Gibt eien Liste aller Übersetzungen wieder aus DB
        /// </summary>
        /// <returns>Unsertützte Sprachen</returns>
        public IEnumerable<string> GetSupportedTranslationsDb()
        {
            return _dao.GetCommonLanguageVos().Select(i => i.Name);
        }

        /// <summary>
        ///  Gibt alle Übersetzungen wieder, die in Excel vorhanden sind
        /// </summary>
        /// <param name="setting"></param>
        /// <returns> Aufzähung von <see cref="ITranslation"/>s</returns>
        public IEnumerable<ITranslation> GetTranslationsExcel(ISetting setting)
        {
          //  Debug.WriteLine("Langauge: " + setting.LanguageExcel);
            var translations = _dao.GetTranslationVos(setting.TranslationSheet)
                .Select(i => new Translation
                {
                    Language = i.Language,
                    Id = i.Id,
                    Dictionary = i.Dictionary
                });

            return translations;
        }

        /// <summary>
        ///  Gibt alle Übersetzungen wieder, die in DB vorhanden sind
        /// </summary>
        /// <returns> Aufzähung von <see cref="ITranslation"/>s</returns>
        public IEnumerable<ITranslation> GetTranslationsDb()
        {
            var translations = _dao.GetCommonTranslateSizingWordVos();
            var languages = _dao.GetCommonLanguageVos();
            var items = new List<ITranslation>();

            var dict = (from l in languages
                join t in translations on l.LangId equals t.LangId into d
                select new {Language = l.Name, Id = l.LangId, Dictionary = d}).ToArray();

            foreach (var d in dict)
            {
                var item = new Translation
                {
                    Id = d.Id,
                    Language = d.Language,
                    Dictionary = new Dictionary<string, string>()
                };

                foreach (var word in d.Dictionary)
                {
                    item.Dictionary.Add(word.WordId.ToString(CultureInfo.InvariantCulture), word.Word);
                }
                items.Add(item);
            }
            return items;
        }

        // todo -->
        public void SyncProductLines()
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "SyncProductLines", "ENTER", DateTime.Now);
            var productLinesExcel = GetProductLinesExcel(_setting);

          
            //language ID
            var langId = SetNewLanguage(_setting.LanguageExcel);

            var translation = GetTranslationsDb().ToArray();

            var productLines = productLinesExcel as IProductLine[] ?? productLinesExcel.ToArray();
            var prepareItems = AddIdsToProductLines(productLines, translation, _setting).ToList();

            //Debug.WriteLine("Items: " + prepareItems.Count);
            //Debug.WriteLine("Lang: " + _setting.LanguageExcel + ", ID: " + langId);
            
            _dao.AddLanguageToDb(_setting.LanguageExcel, langId);

        
            foreach (var productLine in prepareItems)
            {
                Debug.WriteLine(productLine.ToString());
                _dao.AddProductLine(productLine.Id, productLine.Name);

                var propertys = productLine.ProductLineProperties;

                foreach (var p in propertys)
                {
                   // Debug.WriteLine("ProductLine: {0}, property: {1}",productLine.Id, p.Id);
                    _dao.AddProductProperty(p.Id,p.Name,p.GroupId,p.Group);
                    
                    _dao.AddProductLinePropertyRelation(productLine.Id,p.Id, _setting.ConnectionString);
                }

            }

            Debug.WriteLine("###### {0}: {1} ({2}) ", "SyncProductLines", "EXIT", DateTime.Now);
        }

        #endregion

        #region Private Methods


        private IEnumerable<IProductLine> AddIdsToProductLines(IEnumerable<IProductLine> productLines, IEnumerable<ITranslation> translations, ISetting setting )
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddIdsToProductLines", "ENTER", DateTime.Now);
            var items = CleareAllUnsuedProperties(productLines);
            items = AddProductLinesId(items, setting);
            items = AddProductPropertyNameAndGroupId(items, translations, setting);
            // items = AddProductPropertyGroupId(items, translations, setting);
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddIdsToProductLines", "EXIT", DateTime.Now);
            return items;
        }

        /// <summary>
        ///  Filtert alle Propertys raus, die als Value: False makiert sind
        /// </summary>
        /// <param name="productLines">liste alle Produktlines </param>
        /// <returns> Liste aller Prduktlines, ohne Propertys bei deren der Value false ist.</returns>
        private static IEnumerable<IProductLine> CleareAllUnsuedProperties(IEnumerable<IProductLine> productLines)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "CleareAllUnsuedProperties", "ENTER", DateTime.Now);
            var data = productLines.Select(productLine => new ProductLine
            {
                Id = productLine.Id, 
                Name = productLine.Name, 
                ProductLineProperties = productLine.ProductLineProperties.Select(i => new ProductLineProperty
                {
                    Group = i.Group, 
                    GroupId = i.GroupId, 
                    Name = i.Name, 
                    Id = i.Id, 
                    Value = i.Value
                }).Where(j => j.Value)
            }).ToArray();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "CleareAllUnsuedProperties", "EXIT", DateTime.Now);
            return data;
        }

        /// <summary>
        ///  Fügt die ProductLineId der Collection hinzu. und gebt die Items wieder
        /// </summary>
        /// <param name="productLines"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        private IEnumerable<IProductLine> AddProductLinesId(IEnumerable<IProductLine> productLines, ISetting setting)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLinesId", "ENTER", DateTime.Now);
            var items = new List<IProductLine>();
            
            foreach (var productLine in productLines)
            {
                var dbItems = GetProductLinesDb(setting).ToArray();
                if (dbItems.Any(i => i.Name.Contains(productLine.Name)))
                {
                    var item = dbItems.First(i => i.Name.Contains(productLine.Name));
                    items.Add(new ProductLine
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ProductLineProperties = productLine.ProductLineProperties
                    });
                }
                else
                {
                    if (items.Any(i => i.Name.Contains(productLine.Name)))
                        continue;
                    var lastItemId = dbItems[dbItems.Count() - 1].Id;
                    var item = new ProductLine
                    {
                        Id = lastItemId + 1,
                        Name = productLine.Name,
                        ProductLineProperties = productLine.ProductLineProperties
                    };
                    items.Add(item);
                }
            }
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLinesId", "EXIT", DateTime.Now);
            return items.ToArray();
        }



        /// <summary>
        ///  Fügt den ProductLinesProperty Namen und Gruppe eine ID hinzu
        /// </summary>
        /// <param name="productLines"> </param>
        /// <param name="translations"></param>
        /// <param name="setting"></param>
        /// <returns>Aufzählung <see cref="IProductLine"/>s</returns>
        private IEnumerable<IProductLine> AddProductPropertyNameAndGroupId(IEnumerable<IProductLine> productLines, IEnumerable<ITranslation> translations, ISetting setting)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductPropertyNameAndGroupId", "ENTER", DateTime.Now);
            var items = new List<IProductLine>();
            foreach (var productLine in productLines)
            {
              //  var translate = translations as ITranslation[] ?? translations.ToArray();
              //  var productPropertyDb = GetProductLinePropertysDb(productLine.Id, translate, setting);

                var item = new ProductLine
                {
                    Id = productLine.Id,
                    Name = productLine.Name,
                    ProductLineProperties = new List<IProductLineProperty>()
                };

                var properties = new List<IProductLineProperty>();
                // Wenn in der Db keine ProductPropertys vrohanden sind
               // var productLineProperties  = productPropertyDb as IProductLineProperty[] ?? productPropertyDb.ToArray();
               // if (!productLineProperties.Any())
                //{
                    foreach (var productLineProperty in productLine.ProductLineProperties)
                    {
                        //wenn Property Name im Dict vorhanden ist 

                        // Management der ID für die Property
                        int nameId;
                        if (_propertyNameDictionary.Any(i => i.Value.Contains(productLineProperty.Name)))
                        {
                            nameId = _propertyNameDictionary.First(j => j.Value.Contains(productLineProperty.Name)).Key;
                        }
                        else
                        {
                            
                            if (_propertyNameDictionary.Any())
                            {
                                // Id für den Namen
                                var last = _propertyNameDictionary.Last();
                                nameId = 1 + (last.Key);
                                _propertyNameDictionary.Add(nameId, productLineProperty.Name);
                            }
                            else
                            {
                                nameId = 1;
                                _propertyNameDictionary.Add(nameId, productLineProperty.Name);
                            }
                            
                        }

                        // Management für die ID der Gruppe
                        int grpId;
                        if (_propertyGroupDictionary.Any(i => i.Value.Contains(productLineProperty.Group)))
                        {
                            grpId = _propertyGroupDictionary.First(j => j.Value.Contains(productLineProperty.Group)).Key;
                        }
                        else
                        {
                            if (_propertyGroupDictionary.Any())
                            {
                                // Id für die Gruppe
                                var last = _propertyGroupDictionary.Last();
                                grpId = 1 + (last.Key);
                                _propertyGroupDictionary.Add(grpId, productLineProperty.Group);
                            }
                            else
                            {
                                grpId = 1;
                                _propertyGroupDictionary.Add(grpId, productLineProperty.Group);
                            } 
                        }
                       

                        var productProperty = new ProductLineProperty
                        {
                            Id = nameId,
                            Name = productLineProperty.Name,
                            GroupId = grpId,
                            Group = productLineProperty.Group,
                            Value = productLineProperty.Value
                        };
                        properties.Add(productProperty);
                    }

                    item.ProductLineProperties = properties;
                    items.Add(item);
                }
               
           // }
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductPropertyNameAndGroupId", "EXIT", DateTime.Now);
            return items;
          }

        /// <summary>
        ///  Gibt die SprachenID wieder.
        /// </summary>
        /// <param name="languageExcel"> ausgewählte sprach</param>
        /// <returns>
        ///  Ist die Sprache vorhanden, wird die Id der vorhandenen Sprache zurückgegeben,
        ///  andernfalls wird eine neue ID erstellt.
        /// </returns>
        private int SetNewLanguage(string languageExcel)
        {
            var langs = _dao.GetCommonLanguageVos();
            var splitLang = languageExcel.Substring(0, 4);
            var commonLanguageVos = langs as ICommonLanguageVo[] ?? langs.ToArray();
            var containLang = commonLanguageVos.Where(i => i.Name.ToLower().Contains(splitLang.ToLower())).ToArray();
            if (containLang.Any())
            {
                return containLang.First().LangId;
            }
            var lastId = commonLanguageVos[commonLanguageVos.Count() - 1].LangId;
            return ++lastId;
        }
        /// <summary>
        /// Übersetzt word in die eingestellte Spreche
        /// </summary>
        /// <param name="name">Word Name in Excel</param>
        /// <param name="setting">Settings datei</param>
        /// <returns>übersetztes word</returns>
        private string GetTranslation(string name, ISetting setting)
        {
            var trasnlation = _translation ?? (_translation = GetTranslationsExcel(setting).First(i => i.Language.Contains(setting.LanguageExcel)));
            var currentTranslation = trasnlation.Dictionary.First(i => i.Key == name);
            return currentTranslation.Value;
        }

        /// <summary>
        ///  Gibt word anhand der ID und sprache wieder
        /// </summary>
        /// <param name="id">word id</param>
        /// <param name="translations"> setting datei</param>
        /// <returns>übersetzes word</returns>
        private string GetTranslation(int id, IEnumerable<ITranslation> translations)
        {
            var currentDictionary = translations.First().Dictionary;
            // todo: work around
            if (currentDictionary.All(i => i.Key != id.ToString(CultureInfo.InvariantCulture))) return string.Empty; 
            var currentWord = currentDictionary.First(i => i.Key == id.ToString(CultureInfo.InvariantCulture));
            return currentWord.Value;
        }

        /// <summary>
        ///  gibt alle ProduktPopertys zur gehörigen ProductLine wider
        /// </summary>
        /// <param name="productLineId"> Produkt Line Id</param>
        /// <param name="translations">Wörterbuch</param>
        /// <param name="setting">setting</param>
        /// <returns>Ausfzähung von <see cref="IProductLineProperty"/>s</returns>
        private IEnumerable<IProductLineProperty> GetProductLinePropertysDb(int productLineId, IEnumerable<ITranslation> translations, ISetting setting)
        {
            var translationsArray = translations as ITranslation[] ?? translations.ToArray();
            
            var productPropertys = _dao.GetCommonProductPropertyVos().ToArray();
            var productLinePropertyRelation = _dao.GetCommonProductLinePropertyRelationVos(setting.ConnectionString).ToArray();
           
            var result = from plpr in productLinePropertyRelation
                join pp in productPropertys on plpr.PropertyId equals pp.ProductPropertyId
                where plpr.ProductLineId == productLineId
                select
                    new
                    {
                        pp.ProductPropertyGroupId,
                        pp.ProductPropertyNameId,
                        pp.ProductPropertyId,
                        plpr.ProductLineId
                    };

           // var currentProductPropertys = commonProductLinePropertyRelationVos.Where(i => i.ProductLineId == productLineId);
            
        var tmp = result.Select(r => new ProductLineProperty
            {
                Id = r.ProductPropertyId, 
                Name = GetTranslation(r.ProductPropertyNameId, translationsArray), 
              //  NameId = r.ProductPropertyNameId,
                Group = GetPropertyGroupName(r.ProductPropertyGroupId, translationsArray),
                GroupId = r.ProductPropertyGroupId,
                Value = true
            }).ToList();


        return AddAllUnusedProperties(tmp,translationsArray,productPropertys);
        }


        /// <summary>
        ///  Fügt alle Vorhanden Propertys der Porduct Line hinzu, und markiert diese als false
        /// </summary>
        /// <param name="currentProperties"></param>
        /// <param name="translations"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private IEnumerable<IProductLineProperty> AddAllUnusedProperties(
            IEnumerable<IProductLineProperty> currentProperties, IEnumerable<ITranslation> translations,
            IEnumerable<ICommonProductPropertyVo> properties)
        {
            var items = new List<IProductLineProperty>();
            items.AddRange(currentProperties);

            foreach (var prop in properties)
            {
                if (items.Any(j => j.Id == prop.ProductPropertyId)) continue;
                var item = new ProductLineProperty
                {
                    Id = prop.ProductPropertyId,
                    Name = GetTranslation(prop.ProductPropertyNameId, translations),
                   // NameId = prop.ProductPropertyNameId,
                    Group = GetPropertyGroupName(prop.ProductPropertyGroupId, translations),
                    GroupId = prop.ProductPropertyGroupId,
                    Value = false
                };
                items.Add(item);
            }
            return items.OrderBy(i=>i.GroupId).ThenBy(j=>j.Id);
        }


        /// <summary>
        /// Gibt anhand der Id und der Übersetzung den Namen der Property Group wieder
        /// </summary>
        /// <param name="propertyGroupId"></param>
        /// <param name="translations"></param>
        /// <returns>Name der Property Group</returns>
        private string GetPropertyGroupName(int propertyGroupId, IEnumerable<ITranslation> translations)
        {
            var propertyGroup = _dao.GetCommonProductPropertyGroupVos();

            var data = propertyGroup.First(i => i.ProductPropertyGroupId == propertyGroupId);
            
            return GetTranslation(data.ProductPropertyGroupNameId, translations);
        }

        //private IEnumerable<IView> GetViews()
        //{
        //    var views = _dao.GetViewVos(_setting.ViewSheet);
        //    return views.Select(i => new View
        //    {
        //        FieldName = i.FieldName,
        //        OrderId = i.OrderId,
        //        Visible = i.Visible,
        //    });
        //}

        #endregion
    }
}