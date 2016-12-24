using Domain;
using LinqKit;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface IFieldsQueryProcessor
    {
        IEnumerable<DTOs.Field> Search(int? fieldId, string fieldName, int? typeId);

        DTOs.Field GetField(int id);

        DTOs.Field Save(DTOs.Field field);

        DTOs.Field Update(int id, DTOs.Field field);

        void Delete(int id);
    }
    
    public class FieldsQueryProcessor : DBAccessBase<Field>, IFieldsQueryProcessor
    {
        IDecodesQueryProcessor _decodesQueryProcessor;

        public FieldsQueryProcessor(IDecodesQueryProcessor decodesQueryProcessor, ISession session) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        public IEnumerable<DTOs.Field> Search(int? fieldId, string fieldName, int? typeId)
        {
            var filter = PredicateBuilder.New<Field>(x => true);

            if (fieldId.HasValue)
            {
                filter.And(x => x.Id == fieldId);
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                filter.And(x => x.Name.Contains(fieldName));
            }

            if (typeId.HasValue)
            {
                filter.And(x => x.Type.Id == typeId);
            }

            var queryResult = Query().Where(filter);      

            return queryResult.ToList().Select(x =>
            {
                return new DTOs.Field().Initialize(x);
            });
        }

        // TODO handle not found
        public DTOs.Field GetField(int id)
        {
            return new DTOs.Field().Initialize(Get(id));
        }

        public DTOs.Field Save(DTOs.Field field)
        {
            Field newField = new Field
            {
                Name = field.Name,
                Size = _decodesQueryProcessor.Get<FieldSizeDecode>(field.Size),
                Type = _decodesQueryProcessor.Get<FieldTypeDecode>(field.Type),
            };

            Field persistedField = Save(newField);

            return new DTOs.Field().Initialize(persistedField);
        }

        public DTOs.Field Update(int id, DTOs.Field field)
        {
            Field existingField = Get(id);

            existingField.Name = field.Name ?? existingField.Name;

            if (field.Size != 0)
                existingField.Size =  _decodesQueryProcessor.Get<FieldSizeDecode>(field.Size);

            if (field.Type != 0)
                existingField.Type = _decodesQueryProcessor.Get<FieldTypeDecode>(field.Type);

            Update(id, existingField);

            return new DTOs.Field().Initialize(existingField);
        }

        public void Delete(int id)
        {
            Delete(new Field() { Id = id });
        }
    }
}