# UC 1: User creates an account
## Actor: End User

1. User clicks Register Button
2. System takes user to Registration Page
3. User fills data for email, username, password, confirm password
4. User selects register option
5. System gives confirmation that user has successfully registered
6. System takes user to prompting page.

## Variations:
User enters invalid information
a. User selects register option
b. System gives feedback that registration failed
c. System wipes data that user inputed. 

This same flow works for Login

# UC 2: User Uploads a Prompt
## Actor: End User

1. User selects file to upload
2. User selects upload
3. System gives feedback that file upload was successful
4. System takes user to Image Feed

# UC 3: User edits their account
## Actor: End User

1. User clicks on their profile
2. System takes user to their profile page
3. User clicks to edit their profile
4. User enters in new data
5. User clicks to save
6. System gives confirmation that edits were successful

# UC 4: User interacts with a post
## Actor: End User

1. User clicks onto image
2. System expands image and shows information
3. User clicks "like"
4. System gives feedback on like
5. User types data for comment
6. User clicks to send comment
7. System display comment under post
8. User clicks to share
9. System shows a copiable link to image
10. User clicks to save
11. System gives feedback on save
12. User clicks out of post
13. System takes user out to ImageFeed

# UC 5: User adds a friend
## Actor: End User

1. User clicks on another user's profile
2. System takes user to profile page
3. User clicks "add friend"
4. System confirms that a friend request was sent
5. Receiving user accepts the friend request
6. System confirms and shows that the users are now friends
7. System adds the drawings made by the users to their respective daily feed

# UC 6: User searches for another user
## Actor: End User

1. User types a username into the search bar
2. System shows the user all of the matching results

# UC 7: User deletes a post
## Actor: End User

1. User right clicks on the image that they uploaded
2. System shows them options including delete
3. User clicks delete
4. System removes their image from the server and confirms their post has been deleted
5. System returns user to the file upload page

# UC 8: User views previous posts
## Actor: End User

1. User clicks on their profile
2. System takes user to ptofile page
3. User clicks on "previous posts"
4. System takes user to page with previous posts in chronological order
