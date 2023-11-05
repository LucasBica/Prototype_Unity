namespace LB.Http.Runtime.Strapi {

    public class Data<T> where T : EntityData {

        public int id;
        public T attributes;
    }
}