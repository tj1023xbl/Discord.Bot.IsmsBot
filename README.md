# Discord Isms Bot
### A bot designed to store and retrieve user sayings

## Authors 
- TJ Price-Gedrites
- Tyler Dalbora
- Alex Parker

## List Of Commands
- `!add`
   - Used to add new 'isms' to the bot. Add is used on an ismkey followed by the saying in doublequotes. 
   The doublequotes are required.
   - SYNTAX: `!add <USER_ISM> "<USER_SAYING>"`
   - EXAMPLE: `!add tjism "This is a saying that will be stored and retrieved later!"`
 - `!<USER>ism`
   - Used to retrieve a saying via an ismkey. An ism key is of the form: \<user\>ism.
   - SYNTAX: `!<user>ism`
   - EXAMPLE: `!tjism` --> "This is a saying that will be stored and retrieved later!" - TJ
 - `!<USER>ism list`
   - Used to list all saying associated with this user ism key.
   - SYNTAX: `!<user>ism list`
   - EXAMPLE: `!tjism list` --> "This is a saying that will be stored and retrieved later!" - TJ | Added by \<USER\> on \<DATE\>
 - `!random`
   - Used to retrieve a random saying from any user
   - SYNTAX: `!random`
   - EXAMPLE `!random` --> "This is a random saying from a random user" - tyler | Added by TJ on \<DATE\>
