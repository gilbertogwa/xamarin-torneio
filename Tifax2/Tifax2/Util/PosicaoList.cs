using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIFA.Util
{
    public interface IPosicaoChave
    {
        string GetKey();
    }

    public class Posicao<TPosicao> where TPosicao : IPosicaoChave
    {
        public Posicao()
        {

        }

        public Posicao(TPosicao data)
        {
            Data = data;
        }

        public TPosicao Data { get; set; }

        public int Index { get; set; }

    }

    public class PosicaoList<TData> : IEnumerable<TData> where TData : IPosicaoChave
    {

        private readonly List<IPosicaoChave> _list = new List<IPosicaoChave>();
        private readonly Dictionary<string, Posicao<TData>> _dic = new Dictionary<string, Posicao<TData>>();

        public TData this[int index]
        {
            get { return (TData)_list[index]; }
            set { InsertOrUpdate(index, value); }
        }

        public int Count
        {
            get => _list.Count;
        }

        public void InsertRange(IEnumerable<TData> items, Func<TData, int> getActualPosition)
        {

            var listaOrdenada = items.Select(item => new { Item = item, Index = getActualPosition(item) })
                .OrderBy(a => a.Index)
                .ToArray();

            foreach (var item in listaOrdenada)
            {
                if (item == null) continue;
                InsertOrUpdateInternal(item.Index, item.Item, false);
            }

            AtualizarPosicoes();
        }

        public void InsertOrUpdate(int index, TData data)
        {
            InsertOrUpdateInternal(index, data, true);
        }

        private void InsertOrUpdateInternal(int index, TData data, bool atualizarPosicoes)
        {

            if (data == null)
            {
                throw new InvalidOperationException("Valor não pode ser null");
            }

            if (index > _list.Count)
            {
                index = _list.Count;
            }

            if (_dic.TryGetValue(data.GetKey(), out var item))
            {
                item.Data = data;

                if (item.Index == index)
                {
                    _list[index] = data;
                }
                else
                {
                    _list.RemoveAt(item.Index);
                    _list.Insert(index, item.Data);
                    item.Index = index;
                }
            }
            else
            {
                item = new Posicao<TData>() { Data = data };

                _dic.Add(data.GetKey(), item);
                _list.Insert(index, data);
                item.Index = index;

            }

            if (atualizarPosicoes) AtualizarPosicoes();

        }

        public void RemoveByKey(string key)
        {
            if (_dic.TryGetValue(key, out var p) == false) return;
            this.Remove(p.Index);
        }

        public void Remove(int index)
        {

            if (_list.Count <= index) return;

            var item = _list[index];

            _list.RemoveAt(index);
            _dic.Remove(item.GetKey());

            AtualizarPosicoes();

        }

        public void Clear()
        {
            _list.Clear();
            _dic.Clear();
        }

        public Posicao<TData> this[string key]
        {
            get
            {


                if (_dic.TryGetValue(key, out Posicao<TData> item) == false)
                {
                    item = new Posicao<TData>
                    {
                        Index = _list.Count
                    };

                    _dic.Add(key, item);
                    _list.Add(item.Data);

                }

                return item;

            }
            set
            {


                if (_dic.ContainsKey(key) == false)
                {
                    value.Index = _list.Count;
                    _dic.Add(key, value);
                    _list.Add(value.Data);
                }

                _dic[key] = value;

            }
        }

        public IEnumerable<Posicao<TData>> GetPosicoes()
        {
            return _dic.Values;
        }

        public IEnumerator<TData> GetEnumerator()
        {
            return _list.Select(i => (TData)i).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        private void AtualizarPosicoes()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i] == null) continue;
                var key = _list[i].GetKey();
                _dic[key].Index = i;
            }
        }

    }
}
