# Introduction to plaque

- Identify the card type (old paper card vs. new plastic smart card)
- Detect card boundaries and corners (standard ID card dimensions: 85.6mm × 53.98mm)
- Identify the Iranian national card in the image
- Distinguish between old (paper) and new (smart card) versions
- Recognize the standard Iranian national card layout
- Locate the name field (نام و نام خانوادگی)
- Locate the national ID field (کد ملی)
- National ID number (کد ملی)
- 10-digit unique identifier
- Format: XXXXXXXXXX (10 Persian digits)
- Located typically in the upper section of the card
- Full name (نام و نام خانوادگی)
- First and last name in Persian
- Located typically below or near the photo
- May include multiple parts (first name, middle name, last name)
- Confirm national ID is exactly 10 digits
- Verify Persian text encoding is correct for the name
- Use Persian digits (۰-۹) for national ID
- Use proper Persian text encoding (UTF-8) for name
- Preserve original spacing in the name
- If a field is unreadable, use empty string ""
Example Output
{
"nationalId": "۰۰۱۲۳۴۵۶۷۸",
"fullName": "علی احمدی"
}
Privacy and Security Notes
- Handle national card data with strict confidentiality
- Do not store or cache extracted personal information
- Process images securely and delete after extraction
- Comply with Iranian data protection regulations