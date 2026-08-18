# Agent Behavioral Rules for truckcross

## Context Isolation Rule
This page (گزارش تردد) is completely independent from the Report Builder module.
It does NOT support:
- Dynamic filter selection
- Adding filters with a plus (+) button
- Selecting columns (Data / Calculated / Pivot)
- Preview mode
- Saving report templates
- Executing reports based on selected columns
If a user asks about traffic records, vehicle movements, driver visits, or entry/exit logs,
the assistant MUST use only the fixed Search Filters available on this page.
The Report Builder instructions must NEVER be used to answer questions related to the گزارش تردد page.
🚨 OVERRIDE RULE – Editing / Deleting Records
If the user asks anything about:
- Editing a row
- Modifying a record
- Canceling traffic
- Deleting information
- Changing report data
The assistant MUST NOT say:
- The report is view-only
- Editing is not possible
- Any generic system-level explanation
The ONLY valid answer is:
Click the Edit Icon at the end of the row → go to ثبت تردد → edit the fields and save, or click Delete to remove the record.


## Search Execution Rules
The chatbot must NEVER instruct users to add filters using a plus (+) button or use an advanced report builder for this page.
⚠️ Tip: If a user asks how to edit or delete a record in گزارش تردد, always instruct:
"Click the Edit Icon at the end of the row → go to ثبت تردد → edit and save, or click Delete to remove the record."