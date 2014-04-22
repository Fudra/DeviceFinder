using System.Collections.Generic;
using System.Data.SqlTypes;
using Crosscutting.Enums;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Interface.Dao
{
    public interface IDataAccessObject
    {
        /// <summary>
        /// Init DAO
        /// </summary>
        /// <param name="filename"></param>
        void Init(string filename);

        /// <summary>
        /// Lade alle Models aus der Excel datei
        /// </summary>
        /// <returns></returns>
        IEnumerable<IModelVo> GetAllExcelModelVos();

        /// <summary>
        /// Lade alle Produktlinen
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IEnumerable<IProductLineVo> GetAllProductLinesVo(string sheetName);
     
        /// <summary>
        /// Lade alle Models aus der Datenbank
        /// </summary>
        /// <returns></returns>
        IEnumerable<IModelVo> GetAllDbModelsVos();

        /// <summary>
        ///  Lade alle Views aus der Datenbank
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IEnumerable<IViewVo> GetViewVos(string sheetName);

        /// <summary>
        ///  lade alle Translations auder Der Excel Datei
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IEnumerable<ITranslationVo> GetTranslationVos(string sheetName);

        /// <summary>
        /// Unterstützte Sprache aus DB
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonLanguageVo> GetCommonLanguageVos();

        /// <summary>
        /// Produkt Linien 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonProductLineVo> GetCommonProductLineVos();

        /// <summary>
        /// Liest die Realtionen aus der DB FlowConfiguration
        /// </summary>
        /// <param name="conString"></param>
        /// <returns></returns>
        IEnumerable<ICommonProductLinePropertyRelationVo> GetCommonProductLinePropertyRelationVos(string conString);

        /// <summary>
        /// Liest die Produkt Propertys aus DB 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonProductPropertyVo> GetCommonProductPropertyVos();

        /// <summary>
        /// Liest die Produkt Property Gruppe aus DB
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonProductPropertyGroupVo> GetCommonProductPropertyGroupVos();

        /// <summary>
        /// Liest die Übersetzungen aus Db
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonTranslateSizingWordVo> GetCommonTranslateSizingWordVos();

        /// <summary>
        /// Liest die Übersetzungsids aus Db
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommonTranslateSizingWordIdVo> GetCommonTranslateSizingWordIdVos();

       // void WirteModelsToDb(IEnumerable<IModelVo> models, DatabaseWriteModus modus);

        /// <summary>
        ///  Schreibt in die tabelle Common_Language
        /// </summary>
        /// <param name="languages">Language Namen</param>
        /// <param name="id">Language Id</param>
        void AddLanguageToDb(string languages, int id);
        
        /// <summary>
        /// Schreibe in die Tabelle Common_ProductLine
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        void AddProductLine(int id, string name);

        /// <summary>
        /// Schreibe in die Tabelle Common_ProductProperty
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="grpId"></param>
        /// <param name="grpName"></param>
        void AddProductProperty(int id, string name, int grpId, string grpName);

        /// <summary>
        /// Schreibe in die Tabelle Common_ProductLinePropertyRelation
        /// </summary>
        /// <param name="productLineId"></param>
        /// <param name="productPropertyId"></param>
        /// <param name="connectionString"></param>
        void AddProductLinePropertyRelation(int productLineId, int productPropertyId, string connectionString);

    }
}