namespace Capgemini.CodeAnalysis.CoreAnalysers.Models
{
    /// <summary>
    /// Defines implemented analyzers.
    /// </summary>
    public enum AnalyzerType
    {
        /// <summary>
        /// DoNotUse
        /// </summary>
        DoNotUse,

        /// <summary>
        /// ExplicitAccessModifiersAnalyzerId
        /// </summary>
        ExplicitAccessModifiersAnalyzerId,

        /// <summary>
        /// XmlCommentsAnalyzerId
        /// </summary>
        XmlCommentsAnalyzerId,

        /// <summary>
        /// StaticClassAnalyzerId
        /// </summary>
        StaticClassAnalyzerId,

        /// <summary>
        /// NamingConventionAnalyzerId
        /// </summary>
        NamingConventionAnalyzerId,

        /// <summary>
        /// LargeCommentedCodeAnalyzerId
        /// </summary>
        LargeCommentedCodeAnalyzerId,

        /// <summary>
        /// CommentsAnalyzerId
        /// </summary>
        CommentsAnalyzerId,

        /// <summary>
        /// MonsterMethodAnalyzerId
        /// </summary>
        MonsterMethodAnalyzerId,

        /// <summary>
        /// GodClassAnalyzerId
        /// </summary>
        GodClassAnalyzerId,

        /// <summary>
        /// HardCodeAnalyzerId
        /// </summary>
        HardCodeAnalyzerId,

        /// <summary>
        /// FileHierarchyAnalyzerId
        /// </summary>
        FileHierarchyAnalyzerId,

        /// <summary>
        /// OneTypePerFileAnalyzerId
        /// </summary>
        OneTypePerFileAnalyzerId,

        /// <summary>
        /// IfStatementAnalyzerId
        /// </summary>
        IfStatementAnalyzerId,

        /// <summary>
        /// LoopStatementAnalyzerId
        /// </summary>
        LoopStatementAnalyzerId,

        /// <summary>
        /// MethodComplexityAnalyzeId
        /// </summary>
        MethodComplexityAnalyzeId,

        /// <summary>
        /// ConstructorParametersAnalyzerId
        /// </summary>
        ConstructorParametersAnalyzerId,

        /// <summary>
        /// MethodParametersAnalyzerId
        /// </summary>
        MethodParametersAnalyzerId,

        /// <summary>
        /// PrivateFieldNamingUnderscoreAnalyzerId
        /// </summary>
        PrivateFieldNamingUnderscoreAnalyzerId,

        /// <summary>
        /// PrivateFieldNameCasingAnalyzerId
        /// </summary>
        PrivateFieldNameCasingAnalyzerId
    }
}