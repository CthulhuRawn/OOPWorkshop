using System.Collections.Generic;
using System.Linq;
using Domain;
using LinqKit;
using NHibernate;
using Field = EZVet.DTOs.Field;

namespace EZVet.QueryProcessors
{
    public interface IFieldsQueryProcessor
    {
        IEnumerable<Field> Search(int? fieldId, string fieldName, int? typeId);

        Field GetField(int id);

        Field Save(Field field);

        Field Update(int id, Field field);

        void Delete(int id);
    }
    
    public class FieldsQueryProcessor : DBAccessBase<Domain.Field>, IFieldsQueryProcessor
    {
        IDecodesQueryProcessor _decodesQueryProcessor;

        public FieldsQueryProcessor(IDecodesQueryProcessor decodesQueryProcessor, ISession session) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        public IEnumerable<Field> Search(int? fieldId, string fieldName, int? typeId)
        {
            var filter = PredicateBuilder.New<Domain.Field>(x => true);

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
                return new Field().Initialize(x);
            });
        }

        // TODO handle not found
        public Field GetField(int id)
        {
            return new Field().Initialize(Get(id));
        }

        public Field Save(Field field)
        {
            var newField = new Domain.Field
            {
                Name = field.Name,
                Size = _decodesQueryProcessor.Get<FieldSizeDecode>(field.Size),
                Type = _decodesQueryProcessor.Get<FieldTypeDecode>(field.Type)
            };

            var persistedField = Save(newField);

            return new Field().Initialize(persistedField);
        }

        public Field Update(int id, Field field)
        {
            var existingField = Get(id);

            existingField.Name = field.Name ?? existingField.Name;

            if (field.Size != 0)
                existingField.Size =  _decodesQueryProcessor.Get<FieldSizeDecode>(field.Size);

            if (field.Type != 0)
                existingField.Type = _decodesQueryProcessor.Get<FieldTypeDecode>(field.Type);

            Update(id, existingField);

            return new Field().Initialize(existingField);
        }

        public void Delete(int id)
        {
            Delete(new Domain.Field { Id = id });
        }
    }
}