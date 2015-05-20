namespace Tagger.HMM.Business
{
    public interface ITagStatistic
    {
        /// <summary>
        /// Get q(Vt|Dt,JJ), like Q("Dt,JJ,Vt")
        /// </summary>
        /// <param name="tags">tag1,tag2,tag3</param>
        /// <returns></returns>
        double Q(string tags);

        /// <summary>
        /// Get e(base|Vt), like E("Vt|base")
        /// </summary>
        /// <param name="value">tag|word</param>
        /// <returns></returns>
        double E(string value);

        /// <summary>
        /// Get all tags
        /// </summary>
        string[] GetAllTags { get; }
    }
}
