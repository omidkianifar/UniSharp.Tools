using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UniSharp.Tools.Runtime
{
    public interface IFactory<TObject> where TObject : class
    {
        TObject CreateInstance(string type);
        TObject CreateInstance(string type, params object[] args);
    }
    public class Factory<TObject> : IFactory<TObject> where TObject : class
    {
        #region Fields

        protected readonly string _suffixPhrase;

        protected readonly Dictionary<string, Type> _objects
            = new Dictionary<string, Type>();

        #endregion

        public Factory(string suffixPhrase)
        {
            _suffixPhrase = suffixPhrase;

            IEnumerable<Type> types = FetchTypes();

            PrepareDictionary(types);
        }

        #region Public Methods

        public virtual TObject CreateInstance(string objectName)
        {
            if (!_objects.ContainsKey(objectName))
            {
                Debug.LogError($"There is No Game Event Controller With Name : {objectName}");
                return default;
            }

            var objectsType = _objects[objectName];

            var instance = Activator.CreateInstance(objectsType) as TObject;

            return instance;
        }

        public virtual TObject CreateInstance(string objectName, params object[] args)
        {
            if (!_objects.ContainsKey(objectName))
            {
                Debug.LogError($"There is No Game Event Controller With Name : {objectName}");
                return default;
            }

            var objectsType = _objects[objectName];

            var instance = Activator.CreateInstance(objectsType, args) as TObject;

            return instance;
        }

        #endregion

        #region Private Methods

        private IEnumerable<Type> FetchTypes()
        {
            var type = typeof(TObject);

            var types = Assembly.GetAssembly(type)
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => type.IsAssignableFrom(t));
            //.Where(t => t.IsSubclassOf(type));

            return types;
        }

        private void PrepareDictionary(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var className = type.Name.Replace(_suffixPhrase, string.Empty);

                _objects.Add(className, type);
            }
        }

        #endregion
    }
}
