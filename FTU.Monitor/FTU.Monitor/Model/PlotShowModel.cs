using GalaSoft.MvvmLight;
using OxyPlot;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// PlotShowModel 的摘要说明
    /// author: songminghao
    /// date：2017/12/9 14:02:40
    /// desc：波形显示信息类
    /// version: 1.0
    /// </summary>
    public class PlotShowModel : ObservableObject
    {
        /// <summary>
        /// 波形名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 设置和获取波形名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// 显示的波形
        /// </summary>
        private PlotModel _plotModelShow;

        /// <summary>
        /// 设置和获取显示的波形
        /// </summary>
        public PlotModel PlotModelShow
        {
            get
            {
                return this._plotModelShow;
            }
            set
            {
                this._plotModelShow = value;
                RaisePropertyChanged(() => PlotModelShow);
            }
        }

    }
}
