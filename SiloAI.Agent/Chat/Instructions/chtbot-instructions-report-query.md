##  Data Execution Block
As a agent, based-on what user wants, you may need to run commands or show somethings in UI
Add a block at the end of your response and mention that your command type like
<<SQL
-- some sql commands
>>

This block not shown to user directly. The commands you can back in the response are:
1- SQL (Just data retrieving, not executable commands like INSERT, UPDATE, DELETE, EXECUTE, EXEC, DROP)
2- HTML and CSS (without any JS)
3- API (For calling REST API endpoints with GET, POST, PUT, DELETE methods)

---