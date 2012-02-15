#region license

// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Mammock.Impl;
using Mammock.Utilities;

namespace Mammock.Constraints
{

    #region PublicFieldIs

    /// <summary>
    /// Constrain that the public field has a specified value
    /// </summary>
    public class PublicFieldIs : PublicFieldConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicFieldIs"/> class. 
        /// Creates a new <see cref="PublicFieldIs"/> instance.
        /// </summary>
        /// <param name="publicFieldName">
        /// Name of the public field.
        /// </param>
        /// <param name="expectedValue">
        /// Expected value.
        /// </param>
        public PublicFieldIs(string publicFieldName, object expectedValue)
            : base(publicFieldName, Is.Equal(expectedValue))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicFieldIs"/> class. 
        /// Creates a new <see cref="PublicFieldIs"/> instance, specifying a disambiguating
        /// <paramref name="declaringType"/> for the public field.
        /// </summary>
        /// <param name="declaringType">
        /// The type that declares the public field, used to disambiguate between public fields.
        /// </param>
        /// <param name="publicFieldName">
        /// Name of the public field.
        /// </param>
        /// <param name="expectedValue">
        /// Expected value.
        /// </param>
        public PublicFieldIs(Type declaringType, string publicFieldName, object expectedValue)
            : base(declaringType, publicFieldName, Is.Equal(expectedValue))
        {
        }
    }

    #endregion

    #region PublicFieldConstraint

    /// <summary>
    /// Constrain that the public field matches another constraint.
    /// </summary>
    public class PublicFieldConstraint : AbstractConstraint
    {
        /// <summary>
        /// The constraint.
        /// </summary>
        private readonly AbstractConstraint constraint;

        /// <summary>
        /// The declaring type.
        /// </summary>
        private readonly Type declaringType;

        /// <summary>
        /// The public field name.
        /// </summary>
        private readonly string publicFieldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicFieldConstraint"/> class. 
        /// Creates a new <see cref="PublicFieldConstraint"/> instance.
        /// </summary>
        /// <param name="publicFieldName">
        /// Name of the public field.
        /// </param>
        /// <param name="constraint">
        /// Constraint to place on the public field value.
        /// </param>
        public PublicFieldConstraint(string publicFieldName, AbstractConstraint constraint)
            : this(null, publicFieldName, constraint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicFieldConstraint"/> class. 
        /// Creates a new <see cref="PublicFieldConstraint"/> instance, specifying a disambiguating
        /// <paramref name="declaringType"/> for the public field.
        /// </summary>
        /// <param name="declaringType">
        /// The type that declares the public field, used to disambiguate between public fields.
        /// </param>
        /// <param name="publicFieldName">
        /// Name of the public field.
        /// </param>
        /// <param name="constraint">
        /// Constraint to place on the public field value.
        /// </param>
        public PublicFieldConstraint(Type declaringType, string publicFieldName, AbstractConstraint constraint)
        {
            this.declaringType = declaringType;
            this.publicFieldName = publicFieldName;
            this.constraint = constraint;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "public field '" + publicFieldName + "' " + constraint.Message; }
        }

        /// <summary>
        /// Determines if the object passes the constraint.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj == null)
                return false;
            FieldInfo field;

            if (declaringType == null)
            {
                field = obj.GetType().GetField(publicFieldName);
            }
            else
            {
                field = declaringType.GetField(publicFieldName, 
                                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                               BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            if (field == null)
                return false;
            object fieldValue = field.GetValue(obj);
            return constraint.Eval(fieldValue);
        }
    }

    #endregion

    #region PropertyIs

    /// <summary>
    /// Constrain that the property has a specified value
    /// </summary>
    public class PropertyIs : PropertyConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyIs"/> class. 
        /// Creates a new <see cref="PropertyIs"/> instance.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="expectedValue">
        /// Expected value.
        /// </param>
        public PropertyIs(string propertyName, object expectedValue)
            : base(propertyName, Is.Equal(expectedValue))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyIs"/> class. 
        /// Creates a new <see cref="PropertyIs"/> instance, specifying a disambiguating
        /// <paramref name="declaringType"/> for the property.
        /// </summary>
        /// <param name="declaringType">
        /// The type that declares the property, used to disambiguate between properties.
        /// </param>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="expectedValue">
        /// Expected value.
        /// </param>
        public PropertyIs(Type declaringType, string propertyName, object expectedValue)
            : base(declaringType, propertyName, Is.Equal(expectedValue))
        {
        }
    }

    #endregion

    #region PropertyConstraint

    /// <summary>
    /// Constrain that the property matches another constraint.
    /// </summary>
    public class PropertyConstraint : AbstractConstraint
    {
        /// <summary>
        /// The constraint.
        /// </summary>
        private readonly AbstractConstraint constraint;

        /// <summary>
        /// The declaring type.
        /// </summary>
        private readonly Type declaringType;

        /// <summary>
        /// The property name.
        /// </summary>
        private readonly string propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConstraint"/> class. 
        /// Creates a new <see cref="PropertyConstraint"/> instance.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="constraint">
        /// Constraint to place on the property value.
        /// </param>
        public PropertyConstraint(string propertyName, AbstractConstraint constraint)
            : this(null, propertyName, constraint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConstraint"/> class. 
        /// Creates a new <see cref="PropertyConstraint"/> instance, specifying a disambiguating
        /// <paramref name="declaringType"/> for the property.
        /// </summary>
        /// <param name="declaringType">
        /// The type that declares the property, used to disambiguate between properties.
        /// </param>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="constraint">
        /// Constraint to place on the property value.
        /// </param>
        public PropertyConstraint(Type declaringType, string propertyName, AbstractConstraint constraint)
        {
            this.declaringType = declaringType;
            this.propertyName = propertyName;
            this.constraint = constraint;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "property '" + propertyName + "' " + constraint.Message; }
        }

        /// <summary>
        /// Determines if the object passes the constraint.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj == null)
                return false;
            PropertyInfo prop;

            if (declaringType == null)
            {
                prop = obj.GetType().GetProperty(propertyName);
            }
            else
            {
                prop = declaringType.GetProperty(propertyName, 
                                                 BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                                 BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            if (prop == null || !prop.CanRead)
                return false;
            object propertyValue = prop.GetValue(obj, null);
            return constraint.Eval(propertyValue);
        }
    }

    #endregion

    #region TypeOf

    /// <summary>
    /// Constrain that the parameter must be of the specified type
    /// </summary>
    public class TypeOf : AbstractConstraint
    {
        /// <summary>
        /// The type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeOf"/> class. 
        /// Creates a new <see cref="TypeOf"/> instance.
        /// </summary>
        /// <param name="type">
        /// Type.
        /// </param>
        public TypeOf(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "type of {" + type.FullName + "}"; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return type.IsInstanceOfType(obj);
        }
    }

    #endregion

    #region Same

    /// <summary>
    /// Constraint that determines whether an object is the same object as another.
    /// </summary>
    public class Same : AbstractConstraint
    {
        /// <summary>
        /// The same.
        /// </summary>
        private readonly object same;

        /// <summary>
        /// Initializes a new instance of the <see cref="Same"/> class. 
        /// Creates a new <see cref="Equal"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Obj.
        /// </param>
        public Same(object obj)
        {
            this.same = obj;
        }

        /// <summary>
        /// Gets the message for this constraint.
        /// </summary>
        public override string Message
        {
            get
            {
                string sameAsString = (same == null) ? "null" : same.ToString();
                return "same as " + sameAsString;
            }
        }

        /// <summary>
        /// Determines if the object passes the constraints.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return object.ReferenceEquals(same, obj);
        }
    }

    #endregion

    #region Predicate Constraint

    /// <summary>
    /// Evaluate a parameter using constraints
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class PredicateConstraint<T> : AbstractConstraint
    {
        /// <summary>
        /// The predicate.
        /// </summary>
        private readonly Predicate<T> predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateConstraint{T}"/> class. 
        /// Create new instance 
        /// </summary>
        /// <param name="predicate">
        /// </param>
        public PredicateConstraint(Predicate<T> predicate)
        {
            Validate.IsNotNull(predicate, "predicate");
            this.predicate = predicate;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                return string.Format("Predicate ({0})", 
                                     MethodCallUtil.StringPresentation(null, FormatEmptyArgumnet, predicate.Method, 
                                                                       new object[0]));
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj != null &&
                typeof (T).IsAssignableFrom(obj.GetType()) == false)
            {
                throw new InvalidOperationException(
                    string.Format("Predicate accept {0} but parameter is {1} which is not compatible", 
                                  typeof (T).FullName, 
                                  obj.GetType().FullName));
            }

            return predicate((T) obj);
        }

        /// <summary>
        /// The format empty argumnet.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// The format empty argumnet.
        /// </returns>
        private string FormatEmptyArgumnet(Array args, int i)
        {
            return "obj";
        }
    }


    /// <summary>
    /// A constraint based on lambda expression, we are using Expression{T} 
    /// because we want to be able to get good error reporting on that.
    /// </summary>
    public class LambdaConstraint : AbstractConstraint
    {
        /// <summary>
        /// The expr.
        /// </summary>
        private readonly LambdaExpression expr;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaConstraint"/> class.
        /// </summary>
        /// <param name="expr">
        /// The expr.
        /// </param>
        public LambdaConstraint(LambdaExpression expr)
        {
            this.expr = expr;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return expr.ToString(); }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (!IsArgumentTypeIsAssignableFrom(expr, obj))
                return false;

            return (bool) expr.Compile().DynamicInvoke(obj);
        }

        /// <summary>
        /// The is argument type is assignable from.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The is argument type is assignable from.
        /// </returns>
        private bool IsArgumentTypeIsAssignableFrom(LambdaExpression predicate, object obj)
        {
            if (obj != null)
            {
                if (!predicate.Parameters[0].Type.IsAssignableFrom(obj.GetType()))
                {
                    return false;
                }
            }

            return true;
        }
    }

    #endregion

    #region List constraints

    #region Equal

    /// <summary>
    /// Constrain that the list contains the same items as the parameter list
    /// </summary>
    public class CollectionEqual : AbstractConstraint
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly IEnumerable collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEqual"/> class. 
        /// Creates a new <see cref="CollectionEqual"/> instance.
        /// </summary>
        /// <param name="collection">
        /// In list.
        /// </param>
        public CollectionEqual(IEnumerable collection)
        {
            this.collection = collection;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("equal to collection [");
                int i = 0;
                foreach (object o in collection)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(o);
                    i++;
                }

                sb.Append("]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            IEnumerable arg = obj as IEnumerable;
            if (arg != null)
            {
                if (arg is ICollection && collection is ICollection)
                    if (((ICollection) arg).Count != ((ICollection) collection).Count)
                        return false;
                IEnumerator argEnumerator = arg.GetEnumerator(), 
                            collectionEnumerator = collection.GetEnumerator();

                bool argListHasMore = argEnumerator.MoveNext();
                bool constraintListHasMore = collectionEnumerator.MoveNext();
                while (argListHasMore && constraintListHasMore)
                {
                    if (argEnumerator.Current.Equals(collectionEnumerator.Current) == false)
                        return false;
                    argListHasMore = argEnumerator.MoveNext();
                    constraintListHasMore = collectionEnumerator.MoveNext();
                }

                if (argListHasMore || constraintListHasMore)
                    return false;
                return true;
            }

            return false;
        }
    }

    #endregion

    #region OneOf

    /// <summary>
    /// Constrain that the parameter is one of the items in the list
    /// </summary>
    public class OneOf : AbstractConstraint
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private readonly IEnumerable collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneOf"/> class. 
        /// Creates a new <see cref="OneOf"/> instance.
        /// </summary>
        /// <param name="collection">
        /// In list.
        /// </param>
        public OneOf(IEnumerable collection)
        {
            this.collection = collection;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("one of [");
                int i = 0;
                foreach (object o in collection)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(o);
                    i++;
                }

                sb.Append("]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            foreach (object o in collection)
            {
                if (obj.Equals(o))
                    return true;
            }

            return false;
        }
    }

    #endregion

    #region IsIn

    /// <summary>
    /// Constrain that the object is inside the parameter list
    /// </summary>
    public class IsIn : AbstractConstraint
    {
        /// <summary>
        /// The in list.
        /// </summary>
        private readonly object inList;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsIn"/> class. 
        /// Creates a new <see cref="IsIn"/> instance.
        /// </summary>
        /// <param name="inList">
        /// In list.
        /// </param>
        public IsIn(object inList)
        {
            this.inList = inList;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "list contains [" + inList + "]"; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj is IEnumerable)
            {
                foreach (object o in (IEnumerable) obj)
                {
                    if (inList.Equals(o))
                        return true;
                }
            }

            return false;
        }
    }

    #endregion

    #region Count

    /// <summary>
    /// Applies another AbstractConstraint to the collection count.
    /// </summary>
    public class CollectionCount : AbstractConstraint
    {
        /// <summary>
        /// The _constraint.
        /// </summary>
        private readonly AbstractConstraint _constraint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionCount"/> class. 
        /// Creates a new <see cref="CollectionCount"/> instance.
        /// </summary>
        /// <param name="constraint">
        /// The constraint that should be applied to the collection count.
        /// </param>
        public CollectionCount(AbstractConstraint constraint)
        {
            _constraint = constraint;
        }

        /// <summary>
        /// Gets the message for this constraint.
        /// </summary>
        public override string Message
        {
            get { return "collection count " + _constraint.Message; }
        }

        /// <summary>
        /// Determines if the parameter conforms to this constraint.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            ICollection arg = obj as ICollection;

            if (arg != null)
            {
                return _constraint.Eval(arg.Count);
            }

            return false;
        }
    }

    #endregion

    #region Element

    /// <summary>
    /// Applies another AbstractConstraint to a specific list element.
    /// </summary>
    public class ListElement : AbstractConstraint
    {
        /// <summary>
        /// The _constraint.
        /// </summary>
        private readonly AbstractConstraint _constraint;

        /// <summary>
        /// The _index.
        /// </summary>
        private readonly int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListElement"/> class. 
        /// Creates a new <see cref="ListElement"/> instance.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the list element.
        /// </param>
        /// <param name="constraint">
        /// The constraint that should be applied to the list element.
        /// </param>
        public ListElement(int index, AbstractConstraint constraint)
        {
            _index = index;
            _constraint = constraint;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "element at index " + _index + " " + _constraint.Message; }
        }

        /// <summary>
        /// Determines if the parameter conforms to this constraint.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            IList arg = obj as IList;

            if (arg != null)
            {
                if (_index >= 0 && _index < arg.Count)
                    return _constraint.Eval(arg[_index]);
            }

            return false;
        }
    }

    /// <summary>
    /// Applies another AbstractConstraint to a specific generic keyed list element.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class KeyedListElement<T> : AbstractConstraint
    {
        /// <summary>
        /// The _constraint.
        /// </summary>
        private readonly AbstractConstraint _constraint;

        /// <summary>
        /// The _key.
        /// </summary>
        private readonly T _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedListElement{T}"/> class. 
        /// Creates a new <see cref="T:KeyedListElement"/> instance.
        /// </summary>
        /// <param name="key">
        /// The key of the list element.
        /// </param>
        /// <param name="constraint">
        /// The constraint that should be applied to the list element.
        /// </param>
        public KeyedListElement(T key, AbstractConstraint constraint)
        {
            _key = key;
            _constraint = constraint;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "element at key " + _key + " " + _constraint.Message; }
        }

        /// <summary>
        /// Determines if the parameter conforms to this constraint.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod("get_Item", 
                                                            BindingFlags.Public | BindingFlags.GetProperty |
                                                            BindingFlags.DeclaredOnly | BindingFlags.Instance, 
                                                            null, new[] {typeof (T)}, null);

            if (methodInfo != null)
            {
                object value;

                try
                {
                    value = methodInfo.Invoke(obj, new object[] {_key});
                }
                catch (TargetInvocationException)
                {
                    return false;
                }

                return _constraint.Eval(value);
            }

            return false;
        }
    }

    #endregion

    #region ContainsAll Constraint

    /// <summary>
    /// Constrains that all elements are in the parameter list
    /// </summary>
    public class ContainsAll : AbstractConstraint
    {
        /// <summary>
        /// The missing.
        /// </summary>
        private readonly ArrayList missing = new ArrayList();

        /// <summary>
        /// The these.
        /// </summary>
        private readonly IEnumerable these;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsAll"/> class.
        /// </summary>
        /// <param name="these">
        /// The these.
        /// </param>
        public ContainsAll(IEnumerable these)
        {
            this.these = these;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("list missing [");
                int i = 0;
                foreach (object o in missing)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(o);
                    i++;
                }

                sb.Append("]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj is IEnumerable)
            {
                foreach (object outer in these)
                {
                    bool foundThis = false;
                    foreach (object inner in (IEnumerable) obj)
                    {
                        if (inner.Equals(outer))
                        {
                            foundThis = true;
                            break;
                        }
                    }

                    if (!foundThis && !missing.Contains(outer))
                    {
                        missing.Add(outer);
                    }
                }

                return missing.Count == 0;
            }

            return false;
        }
    }

    #endregion

    #endregion

    #region Logic Operator

    #region Or

    /// <summary>
    /// Combines two constraints, constraint pass if either is fine.
    /// </summary>
    public class Or : AbstractConstraint
    {
        /// <summary>
        /// The c 1.
        /// </summary>
        private readonly AbstractConstraint c1;

        /// <summary>
        /// The c 2.
        /// </summary>
        private readonly AbstractConstraint c2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Or"/> class. 
        /// Creates a new <see cref="And"/> instance.
        /// </summary>
        /// <param name="c1">
        /// C1.
        /// </param>
        /// <param name="c2">
        /// C2.
        /// </param>
        public Or(AbstractConstraint c1, AbstractConstraint c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return c1.Message + " or " + c2.Message; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return c1.Eval(obj) || c2.Eval(obj);
        }
    }

    #endregion

    #region Not

    /// <summary>
    /// Negate a constraint
    /// </summary>
    public class Not : AbstractConstraint
    {
        /// <summary>
        /// The c 1.
        /// </summary>
        private readonly AbstractConstraint c1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Not"/> class. 
        /// Creates a new <see cref="And"/> instance.
        /// </summary>
        /// <param name="c1">
        /// C1.
        /// </param>
        public Not(AbstractConstraint c1)
        {
            this.c1 = c1;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "not " + c1.Message; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return !c1.Eval(obj);
        }
    }

    #endregion

    #region And

    /// <summary>
    /// Combines two constraints
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class And : AbstractConstraint
    {
        /// <summary>
        /// The c 1.
        /// </summary>
        private readonly AbstractConstraint c1;

        /// <summary>
        /// The c 2.
        /// </summary>
        private readonly AbstractConstraint c2;

        /// <summary>
        /// Initializes a new instance of the <see cref="And"/> class. 
        /// Creates a new <see cref="And"/> instance.
        /// </summary>
        /// <param name="c1">
        /// C1.
        /// </param>
        /// <param name="c2">
        /// C2.
        /// </param>
        public And(AbstractConstraint c1, AbstractConstraint c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return c1.Message + " and " + c2.Message; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return c1.Eval(obj) && c2.Eval(obj);
        }
    }

    #endregion

    #endregion

    #region String Constraints

    #region Like

    /// <summary>
    /// Constrain the argument to validate according to regex pattern
    /// </summary>
    public class Like : AbstractConstraint
    {
        /// <summary>
        /// The pattern.
        /// </summary>
        private readonly string pattern;

        /// <summary>
        /// The regex.
        /// </summary>
        private readonly Regex regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Like"/> class. 
        /// Creates a new <see cref="Like"/> instance.
        /// </summary>
        /// <param name="pattern">
        /// Pattern.
        /// </param>
        public Like(string pattern)
        {
            regex = new Regex(pattern);
            this.pattern = pattern;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "like \"" + pattern + "\""; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj != null)
            {
                return regex.IsMatch(obj.ToString());
            }

            return false;
        }
    }

    #endregion

    #region Contains

    /// <summary>
    /// Constraint that evaluate whatever an argument contains the specified string.
    /// </summary>
    public class Contains : AbstractConstraint
    {
        /// <summary>
        /// The inner string.
        /// </summary>
        private readonly string innerString;

        /// <summary>
        /// Initializes a new instance of the <see cref="Contains"/> class. 
        /// Creates a new <see cref="Contains"/> instance.
        /// </summary>
        /// <param name="innerString">
        /// Inner string.
        /// </param>
        public Contains(string innerString)
        {
            this.innerString = innerString;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "contains \"" + innerString + "\""; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj != null)
                return obj.ToString().IndexOf(innerString) > -1;
            return false;
        }
    }

    #endregion

    #region Ends With

    /// <summary>
    /// Constraint that evaluate whatever an argument ends with the specified string
    /// </summary>
    public class EndsWith : AbstractConstraint
    {
        /// <summary>
        /// The end.
        /// </summary>
        private readonly string end;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndsWith"/> class. 
        /// Creates a new <see cref="EndsWith"/> instance.
        /// </summary>
        /// <param name="end">
        /// End.
        /// </param>
        public EndsWith(string end)
        {
            this.end = end;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "ends with \"" + end + "\""; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj != null)
                return obj.ToString().EndsWith(end);
            return false;
        }
    }

    #endregion

    #region Starts With

    /// <summary>
    /// Constraint that evaluate whatever an argument start with the specified string
    /// </summary>
    public class StartsWith : AbstractConstraint
    {
        /// <summary>
        /// The start.
        /// </summary>
        private readonly string start;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartsWith"/> class. 
        /// Creates a new <see cref="StartsWith"/> instance.
        /// </summary>
        /// <param name="start">
        /// Start.
        /// </param>
        public StartsWith(string start)
        {
            this.start = start;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "starts with \"" + start + "\""; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj != null)
                return obj.ToString().StartsWith(start);
            return false;
        }
    }

    #endregion

    #endregion

    #region Object Constraints

    #region Equals

    /// <summary>
    /// Constraint that evaluate whatever an object equals another
    /// </summary>
    public class Equal : AbstractConstraint
    {
        /// <summary>
        /// The equal.
        /// </summary>
        private readonly object equal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Equal"/> class. 
        /// Creates a new <see cref="Equal"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Obj.
        /// </param>
        public Equal(object obj)
        {
            this.equal = obj;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                string equalAsString = equal == null ? "null" : equal.ToString();
                return "equal to " + equalAsString;
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj == null)
                return equal == null;
            return Validate.AreEqual(equal, obj);
        }
    }

    #endregion

    #region Anything

    /// <summary>
    /// Constraint that always returns true
    /// </summary>
    public class Anything : AbstractConstraint
    {
        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get { return "anything"; }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            return true;
        }
    }

    #endregion

    #endregion

    #region Math Constraints

    /// <summary>
    /// Constraint that evaluate whatever a comparable is greater than another
    /// </summary>
    public class ComparingConstraint : AbstractConstraint
    {
        /// <summary>
        /// The and equal.
        /// </summary>
        private readonly bool andEqual;

        /// <summary>
        /// The compare to.
        /// </summary>
        private readonly IComparable compareTo;

        /// <summary>
        /// The larger than.
        /// </summary>
        private readonly bool largerThan;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparingConstraint"/> class. 
        /// Creates a new <see cref="ComparingConstraint"/> instance.
        /// </summary>
        /// <param name="compareTo">
        /// The compare To.
        /// </param>
        /// <param name="largerThan">
        /// The larger Than.
        /// </param>
        /// <param name="andEqual">
        /// The and Equal.
        /// </param>
        public ComparingConstraint(IComparable compareTo, bool largerThan, bool andEqual)
        {
            this.compareTo = compareTo;
            this.largerThan = largerThan;
            this.andEqual = andEqual;
        }

        /// <summary>
        /// Gets the message for this constraint
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                string result;
                if (largerThan)
                    result = "greater than ";
                else
                    result = "less than ";
                if (andEqual)
                    result += "or equal to ";
                return result + compareTo;
            }
        }

        /// <summary>
        /// Determines if the object pass the constraints
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The eval.
        /// </returns>
        public override bool Eval(object obj)
        {
            if (obj is IComparable)
            {
                int result = ((IComparable) obj).CompareTo(compareTo);
                if (result == 0 && andEqual)
                    return true;
                if (largerThan)
                    return result > 0;
                else
                    return result < 0;
            }

            return false;
        }
    }

    #endregion
}