import pandas as pd
df = pd.read_parquet('fhvhv_tripdata_2023-08.parquet', engine='pyarrow')
# df.to_feather('fhvhv_tripdata_2023-08.arrow', compression='lz4')
df.to_csv('fhvhv_tripdata_2023-08.csv')

# https://www.nyc.gov/site/tlc/about/tlc-trip-record-data.page
# python.exe -m pip install --upgrade pip
# pip install pyarrow
# pip install pandas

