# ClassName

**Inherits:** [ParentClass](ParentClass.md)  
**Inherited By:** [ChildClassA](ChildClassA.md), [ChildClassB](ChildClassB.md)

One-paragraph description of what this class is and what role it plays in the simulation. State the core responsibility, not the implementation details.

> Remove "Inherited By" if no subclasses exist. Remove "Inherits" only for root classes (rare). Remove any top-level section that has no entries — do not leave empty headings.

---

## Properties

| Type | Name | Default |
|---|---|---|
| [LinkedType](LinkedType.md) | property_name | `value` |
| Double | another_property | — |

> List every public/Friend property. Use `—` for default when there is none or it depends on the constructor. Link types that are project classes. Primitive types (Double, String, Boolean, Long) are not linked.

---

## Methods

| Returns | Signature |
|---|---|
| [ReturnType](ReturnType.md) | **methodName** (param1 As Type, param2 As Type = default) |
| — | **New** (param As Type) |

> "—" return means Sub (no return value). List constructors as **New**. Include optional parameter defaults in the signature. List overloads as separate rows.

---

## Signals

### signalName (param1 As Type, param2 As Type)

Description of when this signal fires and what the parameters contain.

> Omit section if the class raises no events. In VB.NET, "signals" map to `Friend Event` declarations.

---

## Enumerations

### enum EnumName

| Value | Name | Description |
|---|---|---|
| `0` | memberName | What this value means |
| `1` | anotherMember | What this value means |

> One subsection per enum. Include the integer value for all members.

---

## Structures

### structName

| Field | Type | Description |
|---|---|---|
| fieldName | [Type](Type.md) | What this field holds |

> One subsection per structure defined inside this class. Omit if none.

---

## Property Descriptions

### [Type](Type.md) property_name

Full description. Explain valid ranges, side-effects, relationships to other properties, or non-obvious behaviour. Include the backing formula if the property is computed.

```vb
' Example usage if helpful
Dim d As Double = obj.property_name
```

---

## Method Descriptions

### [ReturnType](ReturnType.md) methodName (param1 As Type, param2 As Type = default)

Full description of what the method does, including preconditions, side-effects, and what it returns.

**Parameters**

| Name | Description |
|---|---|
| param1 | What the caller should pass |
| param2 | Optional — defaults to `default` when omitted |

**Returns** — description of the return value, including what `Nothing` means if applicable.

```vb
' Example
Dim result = obj.methodName(start, goal)
```

---

<!--
RULES FOR FILLING THIS TEMPLATE
================================
1. Read the source file before writing — every signature must match the code exactly.
2. Sections with zero entries are deleted entirely (no empty tables, no empty headings).
3. Every project-class type in tables and signatures is a relative Markdown link
   to the corresponding file in docs/classes/.
4. Default column: use the literal default value (e.g. `True`, `0`, `""`)
   or `—` when there is no meaningful default.
5. Properties and methods are listed in the summary tables first (compact),
   then described fully in the Description sections below.
6. Do not copy-paste VB XML doc comments verbatim — rewrite in plain prose.
7. Keep descriptions factual and brief. One paragraph per member is usually enough.
-->
