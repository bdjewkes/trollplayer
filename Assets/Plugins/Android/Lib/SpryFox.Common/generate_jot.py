from jinja2 import Template

out_template = """
        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<
        {%- for i in items -%}
        T{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        >({% for i in items -%}
        T{{i}} msg{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        ) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            {%- for i in items %}
            s_builder.Append(msg{{i}}); {%- if not loop.last %} s_builder.Append(" "); {% endif %}
            {%- endfor %}
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

"""

warn_template = """
        public static void Warn<
        {%- for i in items -%}
        T{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        >({% for i in items -%}
        T{{i}} msg{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        ) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            {%- for i in items %}
            s_builder.Append(msg{{i}}); {%- if not loop.last %} s_builder.Append(" "); {% endif %}
            {%- endfor %}
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.LogWarning(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Warning);
            }
        }

"""

msg_template = """
        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Msg<
        {%- for i in items -%}
        T{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        >(string msgTag, {% for i in items -%}
        T{{i}} msg{{i}} {%- if not loop.last %}, {% endif %}
        {%- endfor -%}
        ) {
            if(s_enabledMsgs.Contains(msgTag)) {
                s_builder.Length = 0;
                s_builder.Append(Time.time).Append(" ");
                s_builder.Append(msgTag);
                s_builder.Append(": ");
                {%- for i in items %}
                s_builder.Append(msg{{i}}); {%- if not loop.last %} s_builder.Append(" "); {% endif %}
                {%- endfor %}
                if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                    Debug.Log(s_builder.ToString());
                }
                if(Callbacks != null) {
                    Callbacks(s_builder.ToString(), msgTag, LogType.Log);
                }
            }
        }

"""

prelude = """using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

namespace SpryFox.Common {

    public partial class Jot {
"""


epilogue = """    }
}"""

with open("Jot.Out.cs", "wb") as f:
    f.write(prelude)

    for size in xrange(1, 15):
        line = Template(out_template).render(items = list(xrange(size)))
        f.write(line)

    f.write(epilogue)

with open("Jot.Warn.cs", "wb") as f:
    f.write(prelude)

    for size in xrange(1, 15):
        line = Template(warn_template).render(items = list(xrange(size)))
        f.write(line)

    f.write(epilogue)

with open("Jot.Msg.cs", "wb") as f:
    f.write(prelude)

    for size in xrange(1, 15):
        line = Template(msg_template).render(items = list(xrange(size)))
        f.write(line)

    f.write(epilogue)