using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
namespace IAU.DTO.Enums
{
    public class GlobalEnum
    {
        public enum Success
        {
            [Description("تم الاضافة بنجاح"), AmbientValue(1)]
            Insert = 1
 ,
            [Description("تم النعديل بنجاح"), AmbientValue(2)]
            Update = 2
,
            [Description("تم الحفظ بنجاح"), AmbientValue(3)]
            Save = 3
 ,
            [Description("تم الحفظ بنجاح"), AmbientValue(4)]
            GetData = 4
 ,
            [Description("تم الحفظ بنجاح"), AmbientValue(5)]
            DeletedScussfuly = 5
        }

        public enum Error
        {
            [Description("Exception!")]
            Exception = -1,
            [Description("There is an error, try again!")]
            Common = 0,

            [Description("Sorry, this record is not exist!"), AmbientValue(10)]
            RecordNotExist = 10,

            [Description("حدث خطأ...حاول مرة اخرى!"), AmbientValue(11)]
            ErrorHappened = 11,

            [Description("خطأ في ادخال البيانات !"), AmbientValue(12)]
            DataValue = 12,

            [Description("هذا السجل موجود من قبل"), AmbientValue(13)]
            RepeatedData = 13,

            [Description("لايمكن الحذف للإرتباط ببيانات اخرى"), AmbientValue(14)]
            RelatedData = 14,

            [Description("أسم المستخدم مسجل من قبل"), AmbientValue(15)]
            RelatedUserName = 15,

            [Description("تأكد من البيانات المطلوبة"), AmbientValue(16)]
            CehckData = 16


        }
        public enum SystemMessage
        {

            [Description("SomeThing Went Wrong"), AmbientValue(0)]
            ExceptionHappened = 0,

            [Description("Added failed"), AmbientValue(1)]
            AddFaild = 1,

            [Description("Added Succeeded"), AmbientValue(2)]
            AddedSucceeded = 2,

            [Description("Updated Succeeded"), AmbientValue(3)]
            UpdatedSucceeded = 3,


            [Description("Updated failed"), AmbientValue(4)]
            Updatedfailed = 4,

            [Description("No Data Found"), AmbientValue(5)]
            NoDataFound = 5,

            [Description("Deleted failed"), AmbientValue(6)]
            Deletedfailed = 6,

            [Description("Deleted Succeeded"), AmbientValue(7)]
            DeletedSucceeded = 7,

            [Description("Published Successfuly"), AmbientValue(8)]
            PublishedSucceeded = 8,

            [Description("Data Dublicated"), AmbientValue(9)]
            DublicatedData = 9,


        }

        public enum RequestState
        {
            [Description("Request is created"), AmbientValue(0)]
            Created = 1,
            [Description("Request In Progress"), AmbientValue(1)]
            PROCESSING = 2,
            [Description("Request is DISPATCHING"), AmbientValue(2)]
            DISPATCHING = 3,
            [Description("Request is DELIVERED"), AmbientValue(2)]
            DELIVERED = 4,
            [Description("Request is Deleted"), AmbientValue(2)]
            Deleted =5,
        }

        public enum RequesterType
        {
            [Description("----"), AmbientValue(0)]
            select = 0,
            [Description("طالب"), AmbientValue(1)]
            Student = 1,

            [Description("ولي امر"), AmbientValue(2)]
            Parent = 2,

            [Description("هيئة تدريس"), AmbientValue(3)]
            Professor = 3,
        }

        public enum StudentType
        {
            [Description("----"), AmbientValue(0)]
            select = 0, [Description("طالب حالي"), AmbientValue(1)]
            StudentCurrent = 1,

            [Description("طالب مستجد"), AmbientValue(2)]
            StudentNew = 2,

            [Description("طالب خريج"), AmbientValue(3)]
            StudentGraduated = 3,
            [Description("طالب منسحب"), AmbientValue(4)]
            StudentWithDrawn = 3,
        }
        public enum RequesterQualification
        {
            [Description("----"), AmbientValue(0)]
            select = 0,
            [Description("بكالوريوس"), AmbientValue(1)]
            Bachelor = 1,

            [Description("دراسات عليا"), AmbientValue(2)]
            HighDegree = 2,
        }

        public enum User_Login
        {
            [Description("تم الدخول"), AmbientValue(1)]
            Valid = 1
,
            [Description("خطأ فى أسم المستخدم أو كلمة المرو"), AmbientValue(2)]
            InvalidCredential = 2,

            [Description("حدث خطأ...حاول مرة اخرى!"), AmbientValue(0)]
            ErrorHappened = 0,

            [Description("تواصل مع مدير النظام التشغيل للاضافة."), AmbientValue(3)]
            ConnectManager = 3,

            [Description("تواصل مع المدير الدومين للاضافة."), AmbientValue(4)]
            ConnectDomain = 4,

            [Description("هذا المستخدم مسجل على جهاز أخر"), AmbientValue(5)]
            LoginFromAnotherDevice = 5,
        }

        public static string GetEnumDescription<T>(object value)
        {
            Type type = typeof(T);
            string name = Enum.GetName(typeof(T), value);
            var enumName = Enum.GetNames(type).Where(f => f.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (enumName == null)
            {
                return string.Empty;
            }
            var field = type.GetField(enumName);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : enumName;
        }

    }
}
